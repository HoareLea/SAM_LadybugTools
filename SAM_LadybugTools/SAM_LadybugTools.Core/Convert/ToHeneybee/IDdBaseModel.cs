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

            //string className = Query.ClassName(@object);
            //if(string.IsNullOrWhiteSpace(className))
            //{
            //    return null;
            //}

            string json = ToString(@object);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return ToHoneybee(json);

            //switch (className)
            //{
            //    case "Room":
            //        return Room.FromJson(json);

            //    case "Model":
            //        return Model.FromJson(json);
            //}

            //return null;
        }

        public static IDdBaseModel ToHoneybee(string json)
        {
            System.Text.Json.JsonDocument jsonDocument = null;

            try
            {
                jsonDocument = System.Text.Json.JsonDocument.Parse(json);
            }
            catch
            {
                jsonDocument = null;
            }

            return ToHoneybee(jsonDocument);
        }

        public static IDdBaseModel ToHoneybee(this System.Text.Json.JsonDocument jsonDocument)
        {
            if (jsonDocument == null)
            {
                return null;
            }

            if(!jsonDocument.RootElement.TryGetProperty("type", out System.Text.Json.JsonElement jsonElement) || jsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
            {
                return null;
            }

            string type = jsonElement.GetString();
            if(string.IsNullOrWhiteSpace(type))
            {
                return null;
            }

            string json = ToString(jsonDocument, false);
            if(string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            switch(type)
            {
                case "Room":
                    return Room.FromJson(json);

                case "Model":
                    return Model.FromJson(json);
            }

            throw new System.NotImplementedException();
        }
    }
}