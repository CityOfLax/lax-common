namespace Lax.Caching.Memory.Namespaced {

    internal class NamespacedMemoryCacheEntryKey<TKey, TValue> {
        
        public string Namespace { get; }

        public TKey Key { get; }

        public NamespacedMemoryCacheEntryKey(string namespc, TKey key) {
            Namespace = namespc;
            Key = key;
        }

        public override bool Equals(object obj) {
            if (GetType().IsInstanceOfType(obj)) {
                var castObj = obj as NamespacedMemoryCacheEntryKey<TKey, TValue>;
                return castObj != null &&
                       castObj.Namespace.Equals(Namespace) &&
                       castObj.Key.Equals(Key);
            }
            return false;
        }

        public override int GetHashCode() {
            int hashCode = 7;
            hashCode = hashCode*31 + typeof(TKey).GetHashCode();
            hashCode = hashCode*31 + typeof(TValue).GetHashCode();
            hashCode = hashCode*31 + Namespace.GetHashCode();
            hashCode = hashCode*31 + Key.GetHashCode();
            return hashCode;
        }

    }

}