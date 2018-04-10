namespace Sitecore.Support.XA.Foundation.Search.FieldReaders
{
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.FieldReaders;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using System.Collections.Generic;
  using System.Linq;

  public class NameLookupListFieldReader : FieldReader
  {
    public override object GetFieldValue(IIndexableDataField indexableField)
    {
      MultilistField multilistField = FieldTypeManager.GetField(indexableField as SitecoreItemDataField) as MultilistField;
      if (multilistField == null)
      {
        return new List<string>();
      }
      return (from i in multilistField.GetItems()
              where i != null
              select i.Name).ToList();
    }
  }
}