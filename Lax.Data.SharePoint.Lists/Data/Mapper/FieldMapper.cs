﻿using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Converters;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data.Mapper {

    /// <summary>
    /// Represents class that can map value from SP list field to the specified entity member.
    /// </summary>
    /// <typeparam name="TSPItem">Exact type of SP list item, i.e. SPListItem for SSOM, ListItem for CSOM.</typeparam>
    [PublicAPI]
    public sealed class FieldMapper<TSPItem> {

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapper{TSPItem}"/>.
        /// </summary>
        /// <param name="field">Field metadata.</param>
        /// <param name="storeAccessor">SP list field accessor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="field"/> or <paramref name="storeAccessor"/> is null.</exception>
        public FieldMapper([NotNull] MetaField field, [NotNull] IFieldAccessor<TSPItem> storeAccessor) {
            Guard.CheckNotNull(nameof(field), field);
            Guard.CheckNotNull(nameof(storeAccessor), storeAccessor);

            Field = field;
            MemberAccessor = new MemberAccessor(field.Member);
            StoreAccessor = storeAccessor;
        }

        /// <summary>
        /// Gets <see cref="MetaField"/> that is associated with current mapper.
        /// </summary>
        [NotNull]
        public MetaField Field { get; }

        /// <summary>
        /// Gets field or property accessor.
        /// </summary>
        [NotNull]
        public IFieldAccessor<object> MemberAccessor { get; }

        /// <summary>
        /// Gets SP list field accessor.
        /// </summary>
        [NotNull]
        public IFieldAccessor<TSPItem> StoreAccessor { get; }

        /// <summary>
        /// Gets <see cref="IFieldConverter"/> associated with current <see cref="Field"/>.
        /// </summary>
        public IFieldConverter Converter => Field.Converter;

        /// <summary>
        /// Maps entity member to SP list field.
        /// </summary>
        /// <param name="source">Source entity to map.</param>
        /// <param name="dest">Destination SP list item.</param>
        /// <exception cref="DataMappingException">Cannot map or convert value.</exception>
        public void Map(object source, TSPItem dest, ClientContext clientContext) {
            if (!MemberAccessor.CanGetValue || !StoreAccessor.CanSetValue) {
                return;
            }

            try {
                var clrValue = MemberAccessor.GetValue(source);
                var clientValue = Converter.ToSpValue(clrValue);

                StoreAccessor.SetValue(dest, clientValue);

                //if (clientValue != null && (clientValue.GetType() == typeof(TaxonomyFieldValue) || clientValue.GetType().IsAssignableFrom(typeof(List<TaxonomyFieldValue>)))) { 

                //    var listItem = dest as ListItem;

                //    var baseField = listItem.ParentList.Fields.GetByInternalNameOrTitle(Field.InternalName);

                //    clientContext.Load(baseField);
                //    clientContext.ExecuteQuery();

                //    var taxonomyField = baseField.TypedObject as TaxonomyField;

                //    if (taxonomyField != null) {
                //        if (clientValue.GetType() == typeof(TaxonomyFieldValue)) {
                //            taxonomyField.SetFieldValueByValue(listItem, clientValue as TaxonomyFieldValue);
                //        } else {
                //            var termCollection = new TaxonomyFieldValueCollection(clientContext, baseField);
                //        }
                //    }
                    

                //} else {
                //    StoreAccessor.SetValue(dest, clientValue);
                //}

                
            } catch (Exception e) {
                throw Error.CannotMapFieldToSP(Field, e);
            }
        }

        /// <summary>
        /// Maps entity member to CAML value.
        /// </summary>
        /// <param name="source">Source entity to map.</param>
        /// <exception cref="DataMappingException">Cannot map or convert value.</exception>
        public string MapToCaml(object source) {
            if (!MemberAccessor.CanGetValue) {
                return null;
            }

            try {
                var clrValue = MemberAccessor.GetValue(source);
                return Converter.ToCamlValue(clrValue);
            } catch (Exception e) {
                throw Error.CannotMapFieldToSP(Field, e);
            }
        }

        /// <summary>
        /// Maps SP list field to entity member.
        /// </summary>
        /// <param name="source">Source SP list item to map.</param>
        /// <param name="dest">Destination entity.</param>
        /// <exception cref="DataMappingException">Cannot map or convert value.</exception>
        public void Map(TSPItem source, object dest) {
            if (!StoreAccessor.CanGetValue || !MemberAccessor.CanSetValue) {
                return;
            }

            try {
                var clientValue = StoreAccessor.GetValue(source);
                var clrValue = Converter.FromSpValue(clientValue);
                MemberAccessor.SetValue(dest, clrValue);
            } catch (Exception e) {
                throw Error.CannotMapFieldFromSP(Field, e);
            }
        }

    }

}