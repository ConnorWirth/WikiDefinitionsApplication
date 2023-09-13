using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDefinitionsApplication
{
    internal class Information
    {
        public class WikiData : IComparable<WikiData>
        {
            // Private attributes
            private string Name;
            private string Category;
            private string Structure;
            private string Definition;

            // Public getters and setters for Name
            public string GetName()
            {
                return Name;
            }

            public void SetName(string name)
            {
                Name = name;
            }

            // Public getters and setters for Category
            public string GetCategory()
            {
                return Category;
            }

            public void SetCategory(string category)
            {
                Category = category;
            }

            // Public getters and setters for Structure
            public string GetStructure()
            {
                return Structure;
            }

            public void SetStructure(string structure)
            {
                Structure = structure;
            }

            // Public getters and setters for Definition
            public string GetDefinition()
            {
                return Definition;
            }

            public void SetDefinition(string definition)
            {
                Definition = definition;
            }

            // CompareTo method for IComparable<T> interface
            public int CompareTo(WikiData other)
            {
                // Implement comparison logic here (e.g., compare by Name)
                return Name.CompareTo(other.Name);
            }
        }
    }
}
