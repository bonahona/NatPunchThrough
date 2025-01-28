namespace NatLibrary.Extensions {
    public static class ObjectExtension {
        public static bool In<T> (this T obj, params T[] values) {
            return values.ToList().Contains (obj);
        }
    }
}
