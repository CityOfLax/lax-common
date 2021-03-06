﻿using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Data.Mapper;
using Lax.Data.SharePoint.Lists.Data.QueryModels;
using Lax.Data.SharePoint.Lists.Data.Translators;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents base class for SP list items provider.
    /// </summary>
    public abstract class BaseSpListItemsProvider<TSPListItem> : ISpListItemsProvider {

        private const int BatchSize = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpListItemsProvider{T}" />
        /// </summary>
        /// <param name="list">Meta models of the SP list.</param>
        protected BaseSpListItemsProvider([NotNull] MetaList list) {
            Guard.CheckNotNull(nameof(list), list);

            List = list;
        }

        /// <summary>
        /// Gets list associated with this instance of the <see cref="BaseSpListItemsProvider{T}"/>.
        /// </summary>
        public MetaList List { get; }

        public bool FilterByContentType { get; set; }

        public IEnumerable<T> Fetch<T>(QueryModel query) {
            var contentType = List.ContentTypes[typeof(T)];
            UpdateViewFields(query, contentType);
            var viewFields = query.SelectableKnownFields.EmptyIfNull().ToList();
            var caml = ConvertToCamlString(query, contentType);

            return Materialize<T>(FetchInternal(caml), contentType, viewFields);
        }

        public bool Any<T>(QueryModel query) {
            var contentType = List.ContentTypes[typeof(T)];
            query.ReplaceSelectableFields(new[] {new KeyRefModel()});
            var caml = ConvertToCamlString(query, contentType);

            return FetchInternal(caml).Any();
        }

        public int Count<T>(QueryModel query) {
            var contentType = List.ContentTypes[typeof(T)];
            query.ReplaceSelectableFields(new[] {new KeyRefModel()});
            var caml = ConvertToCamlString(query, contentType);

            return FetchInternal(caml).Count();
        }

        public T SingleOrDefault<T>(QueryModel query) {
            var contentType = List.ContentTypes[typeof(T)];

            query.RowLimit = 2;
            UpdateViewFields(query, contentType);

            var viewFields = query.SelectableKnownFields.EmptyIfNull().ToList();
            var caml = ConvertToCamlString(query, contentType);

            var foundItems = FetchInternal(caml).ToList();
            if (foundItems.Count > 1) {
                throw Error.MoreThanOneMatch();
            }
            return foundItems.Count == 1
                ? Materialize<T>(foundItems[0], contentType, viewFields)
                : default(T);
        }

        public T FirstOrDefault<T>(QueryModel query) {
            var contentType = List.ContentTypes[typeof(T)];

            query.RowLimit = 1;
            UpdateViewFields(query, contentType);

            var viewFields = query.SelectableKnownFields.EmptyIfNull().ToList();
            var caml = ConvertToCamlString(query, contentType);

            var foundItems = FetchInternal(caml).ToList();
            return foundItems.Count == 1
                ? Materialize<T>(foundItems[0], contentType, viewFields)
                : default(T);
        }

        public T ElementAtOrDefault<T>(QueryModel query, int index) {
            var contentType = List.ContentTypes[typeof(T)];

            query.RowLimit = index + 1;
            UpdateViewFields(query, contentType);

            var viewFields = query.SelectableKnownFields.EmptyIfNull().ToList();
            var caml = ConvertToCamlString(query, contentType);

            var foundItem = FetchInternal(caml).ElementAtOrDefault(index);
            return foundItem != null
                ? Materialize<T>(foundItem, contentType, viewFields)
                : default(T);
        }

        public T Get<T>(int id) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            return Materialize<T>(GetInternal(id, contentType), contentType);
        }

        public T Add<T>(T item) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            return (T) AddInternal(item, contentType.GetMapper<TSPListItem>());
        }

        public void Add<T>(IEnumerable<T> items) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            var batches = items.ToPages(BatchSize);
            foreach (var batch in batches) {
                AddInternal((IEnumerable<object>) batch, contentType.GetMapper<TSPListItem>());
            }
        }

        public T Update<T>(T item) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            var idValue = (int) idField
                .GetMapper<TSPListItem>()
                .MemberAccessor
                .GetValue(item);

            return (T) UpdateInternal(idValue, item, contentType.GetMapper<TSPListItem>());
        }

        public void Update<T>(IEnumerable<T> items) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            var idValueAccessor = idField
                .GetMapper<TSPListItem>()
                .MemberAccessor;

            var itemsToAdd = items.Select(n => new KeyValuePair<int, object>((int) idValueAccessor.GetValue(n), n));

            var batches = itemsToAdd.ToPages(BatchSize);
            foreach (var batch in batches) {
                UpdateInternal(batch, contentType.GetMapper<TSPListItem>());
            }
        }

        public void Delete<T>(T item) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            var idValue = (int) idField
                .GetMapper<TSPListItem>()
                .MemberAccessor
                .GetValue(item);

            DeleteInternal(idValue);
        }

        public void Delete<T>(IEnumerable<T> items) {
            if (List.IsExternal) {
                throw Error.OperationNotAllowedForExternalList();
            }

            var contentType = List.ContentTypes[typeof(T)];
            var idField = contentType.GetKeyField();

            if (idField == null) {
                throw Error.OperationRequireIdField();
            }

            var idValueAccessor = idField
                .GetMapper<TSPListItem>()
                .MemberAccessor;

            var batches = items
                .Select(n => (int) idValueAccessor.GetValue(n))
                .ToPages(BatchSize);

            foreach (var batch in batches) {
                DeleteInternal(batch);
            }
        }

        public abstract IEnumerable<string> GetAttachments(int id);

        /// <summary>
        /// Converts query model to CAML-string in next format <![CDATA[<View><Query></Query></View>]]>.
        /// </summary>
        /// <param name="queryModel">Query model to convert.</param>
        /// <param name="contentType">Content type to use as a fields source.</param>
        /// <returns>CAML-string in next format <![CDATA[<View><Query></Query></View>]]></returns>
        protected string ConvertToCamlString([NotNull] QueryModel queryModel, [NotNull] MetaContentType contentType) {
            if (FilterByContentType && !List.IsExternal) {
                queryModel.MergeWheres(new ComparisonModel(ComparisonOperator.Eq, new ContentTypeIdRefModel(),
                    contentType.Id) {
                    IsValueConverted = true
                });
            }

            return new CamlQueryTranslator(contentType).Process(queryModel);
        }

        /// <summary>
        /// Updates view fields in query if they were't specified.
        /// </summary>
        /// <param name="query">Query to update view fields.</param>
        /// <param name="contentType">Content type that provides list of fields to load.</param>
        protected void UpdateViewFields([NotNull] QueryModel query, [NotNull] MetaContentType contentType) {
            if (!query.SelectableFields.IsNullOrEmpty()) {
                return;
            }

            var viewFields = ((IEnumerable<MetaField>) contentType.Fields)
                .Select(n => new MemberRefModel(n.Member))
                .ToList();

            query.MergeSelectableFields(viewFields);
        }

        /// <summary>
        /// Fetches SP list items that match to specified caml string.
        /// </summary>
        /// <param name="caml">CAML query string.</param>
        /// <returns>Loaded SP list items.</returns>
        protected abstract IEnumerable<TSPListItem> FetchInternal(string caml);

        /// <summary>
        /// Fetchs SP list item by specified ID.
        /// </summary>
        /// <param name="id">Item ID to load.</param>
        /// <param name="contentType">Expected item content type info.</param>
        /// <returns>Loaded SP list item.</returns>
        protected abstract TSPListItem GetInternal(int id, MetaContentType contentType);

        /// <summary>
        /// Adds item to SP list.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <param name="mapper">Mapper to SP list item.</param>
        /// <returns>New SP list item ID.</returns>
        protected abstract object AddInternal(object item, TypeMapper<TSPListItem> mapper);

        /// <summary>
        /// Adds items to SP list.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <param name="mapper">Mapper to SP list item.</param>
        /// <param name="clientContext"></param>
        /// <returns>New SP list item ID.</returns>
        protected abstract void AddInternal(IEnumerable<object> items, TypeMapper<TSPListItem> mapper, ClientContext clientContext);

        /// <summary>
        /// Updates item in SP list.
        /// </summary>
        /// <param name="id">Item ID to update.</param>
        /// <param name="item">Item to update.</param>
        /// <param name="mapper">Mapper to SP list item.</param>
        /// <param name="clientContext"></param>
        protected abstract object UpdateInternal(int id, object item, TypeMapper<TSPListItem> mapper);

        /// <summary>
        /// Updates items in SP list.
        /// </summary>
        /// <param name="items">Items to update.</param>
        /// <param name="mapper">Mapper to SP list item.</param>
        /// <param name="clientContext"></param>
        protected abstract void UpdateInternal(IEnumerable<KeyValuePair<int, object>> items, TypeMapper<TSPListItem> mapper);

        /// <summary>
        /// Deletes item from SP list.
        /// </summary>
        /// <param name="id">Item ID to delete.</param>
        protected abstract void DeleteInternal(int id);

        /// <summary>
        /// Deletes items from SP list.
        /// </summary>
        /// <param name="ids">Items IDs to delete.</param>
        protected abstract void DeleteInternal(IEnumerable<int> ids);

        /// <summary>
        /// Creates native object from SP list item.
        /// </summary>
        /// <typeparam name="T">Type of native object.</typeparam>
        /// <param name="spItem">SP list item to instantiate and map.</param>
        /// <param name="contentType">Content type model.</param>
        /// <param name="fields">Viewable fields.</param>
        /// <returns>Native object.</returns>
        protected T Materialize<T>(TSPListItem spItem, MetaContentType contentType,
            IReadOnlyCollection<MemberRefModel> fields = null) {
            var mapper = contentType.GetMapper<TSPListItem>();

            return (T) mapper.CreateAndMap(spItem, fields);
        }

        /// <summary>
        /// Creates native objects from SP list items.
        /// </summary>
        /// <typeparam name="T">Type of native object.</typeparam>
        /// <param name="spItems">SP list items to instantiate and map.</param>
        /// <param name="contentType">Content type model.</param>
        /// <param name="fields">Viewable fields.</param>
        /// <returns>Collection of native object.</returns>
        protected IEnumerable<T> Materialize<T>(IEnumerable<TSPListItem> spItems, MetaContentType contentType,
            IReadOnlyCollection<MemberRefModel> fields = null) {
            var mapper = contentType.GetMapper<TSPListItem>();

            return mapper.CreateAndMap<T>(spItems, fields);
        }

    }

}