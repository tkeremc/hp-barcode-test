namespace HPBarcodeTest.Helpers;

public static class UpdateCheckHelper
{
    public static T Checker<T>(T oldModel, T newModel) where T : class
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(oldModel);
            var newValue = property.GetValue(newModel);
            
            if (newValue == null || (newValue is string stringValue && string.IsNullOrEmpty(stringValue)))
                property.SetValue(newModel, oldValue);
        }
        return newModel;
    }
}