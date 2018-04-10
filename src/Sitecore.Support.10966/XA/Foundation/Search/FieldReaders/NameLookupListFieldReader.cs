namespace Sitecore.Support.XA.Foundation.Search.FieldReaders
{
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.FieldReaders;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;

  public class NameLookupListFieldReader : FieldReader
  {
    public override object GetFieldValue(IIndexableDataField indexableField)
    {
      MultilistField multilistField = FieldTypeManager.GetField(indexableField as SitecoreItemDataField) as MultilistField;
      if (multilistField == null)
      {
        return new List<string>();
      }
      #region Modified code
      Field field = indexableField as SitecoreItemDataField;
      var lang = field.Language;
      return multilistField.GetItems().Where(i => i != null).Select(
          i =>
          {
            if (lang == i.Language || i.Languages.All(l => l != lang))
            {
              return GetDisplayName(i);
            }
            var langItem = i.Versions.GetLatestVersion(lang);
            return GetDisplayName(langItem);
          }).ToList();
      #endregion
    }
    #region Added code
    protected virtual string GetDisplayName(Item item)
    {
      string title = item[Tag.Fields.Title];
      if (!string.IsNullOrWhiteSpace(title))
      {
        return title;
      }
      if (!string.IsNullOrEmpty(item.DisplayName))
      {
        return item.DisplayName;
      }

      return item.Name;
    }
    private struct Tag
    {
      public static ID ID = ID.Parse("{6B40E84C-8785-49FC-8A10-6BCA862FF7EA}");
      public struct Fields
      {
        public static ID Title = ID.Parse("{B8A8B6C9-4757-496B-9AAE-5F46FF381563}");
      }
    }
    #endregion
  }
}