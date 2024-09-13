
using TaskTry.Models;

namespace TaskTry.Mappers
{
    public static class ItemsMapper
    {
        public static List<Item> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<Item>();
            int count = -1;
            foreach (var value in values)
            {
                count++;
                if (count == 0) continue;
                Item item = new()
                {

                    Name = value[0].ToString(),
                    DateOfBirth = value[1].ToString(),
                    ResidentialAddress = value[2].ToString(),
                    PermanentAddress = value[3].ToString(),
                    PhoneNumber = value[4].ToString(),
                    EmailAddress = value[5].ToString(),
                    MaritalStatus = value[6].ToString(),
                    Gender = value[7].ToString(),
                    Occupation = value[8].ToString(),
                    AadharCardNumber = value[9].ToString(),
                    PANNumber = value[10].ToString()
                };




                items.Add(item);
            }

            return items;
        }

        public static List<Item> MapFromId(IList<IList<object>> values)
        {
            var items = new List<Item>();
            foreach (var value in values)
            {
                Item item = new()
                {

                    Name = value[0].ToString(),
                    DateOfBirth = value[1].ToString(),
                    ResidentialAddress = value[2].ToString(),
                    PermanentAddress = value[3].ToString(),
                    PhoneNumber = value[4].ToString(),
                    EmailAddress = value[5].ToString(),
                    MaritalStatus = value[6].ToString(),
                    Gender = value[7].ToString(),
                    Occupation = value[8].ToString(),
                    AadharCardNumber = value[9].ToString(),
                    PANNumber = value[10].ToString()
                };
                items.Add(item);
            }
            return items;
        }

        public static IList<IList<object>> MapToRangeData(Item item)
        {
            var objectList = new List<object>() { item.Name, item.DateOfBirth, item.ResidentialAddress, item.PermanentAddress, item.PhoneNumber, item.EmailAddress, item.MaritalStatus, item.Gender, item.Occupation, item.AadharCardNumber, item.PANNumber };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}