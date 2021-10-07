using HoneybeeSchema;

namespace SAM.Core.LadybugTools
{
    public static partial class Convert
    {
        public static IDdBaseModel ToHoneybee(this object @object)
        {
            if(@object == null)
            {
                return null;
            }

            string className = Query.ClassName(@object);
            if(string.IsNullOrWhiteSpace(className))
            {
                return null;
            }

            string json = ToString(@object);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            switch(className)
            {
                case "Room":
                    return Room.FromJson(json);

                case "Model":
                    return Model.FromJson(json);
            }

            return null;
        }
    }
}