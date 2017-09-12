using System.Reflection;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data.QueryModels {

    /// <summary>
    /// Represents FieldRef tag in CAML query that associated with the specified <see cref="MemberInfo"/>.
    /// </summary>
    public sealed class MemberRefModel : FieldRefModel {

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRefModel"/> for the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member">Entity member</param>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is null.</exception>
        public MemberRefModel([NotNull] MemberInfo member)
            : base(FieldRefType.KnownMember) {
            Guard.CheckNotNull(nameof(member), member);

            Member = member;
        }

        /// <summary>
        /// Gets <see cref="MemberInfo"/> that associated with current FieldRef.
        /// </summary>
        [NotNull]
        public MemberInfo Member { get; }

        /// <summary>
        /// Returns a <see cref="String"/> which represents the object instance.
        /// </summary>
        /// <returns>CAML-like string.</returns>
        public override string ToString() {
            return $"<FieldRef Name='{Member.Name}' />";
        }

    }

}