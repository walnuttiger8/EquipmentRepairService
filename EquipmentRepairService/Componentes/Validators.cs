using System;

namespace EquipmentRepairService.Componentes
{
    public static class Validators
    {
        public static Func<object, string> TypeOf(Type type, string errorMessage)
        {
            return (object value) => 
            { 
                try
                {
                    Convert.ChangeType(value, type);
                    return null;
                } catch 
                {
                    return errorMessage;
                }
            };
        }

        public static Func<object, string> TypeOfInt(string errorMessage)
        {
            return (object value) =>
            {
                try
                {
                    Convert.ToInt32(value);
                    return null;
                }
                catch (Exception ex)
                {
                    return errorMessage;
                }
            };
        }


        public static Func<object, string> MinLength(int minLength, string errorMessage)
        {
            return (object value) =>
            {
                if (value.ToString().Length < minLength)
                {
                    return errorMessage;
                }
                return null;
            };
        }
    }
}
