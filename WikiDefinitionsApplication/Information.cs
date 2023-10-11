using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDefinitionsApplication
{
    internal class Information : IComparable<Information>
    {
        /* 
        6.1 Create a separate class file to hold the four data items of the Data Structure(use the Data Structure Matrix as a guide).
        Use private properties for the fields which must be of type “string”. The class file must have separate setters and getters,
        add an appropriate IComparable for the Name attribute.Save the class as “Information.cs”.
        */

        // Private attributes
        private string name;
        private string category;
        private string structure;
        private string definition;

        // Public getters and setters for Name
        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName()
        {
            return name;
        }

        // Public getters and setters for Category
        public void SetCategory(string newCategory)
        {
            category = newCategory;
        }

        public string GetCategory()
        {
            return category;
        }


        // Public getters and setters for Structure
        public void SetStructure(string newStructure)
        {
            structure = newStructure;
        }

        public string GetStructure()
        {
            return structure;
        }

        // Public getters and setters for Definition
        public void SetDefinition(string newDefinition)
        {
            definition = newDefinition;
        }

        public string GetDefinition()
        {
            return definition;
        }

        // CompareTo method for IComparable<T> interface
        public int CompareTo(Information other)
        {
            // Implement comparison logic here (e.g., compare by Name)
            return name.CompareTo(other.name);
        }

        // Constructor for the Information class that takes four parameters to initialize its fields.
        public Information(string name, string category, string structure, string definition)
        {
            // Assign the provided values to the corresponding fields of the Information object.
            this.name = name;
            this.category = category;
            this.structure = structure;
            this.definition = definition;
        }

        // Default constructor for the Information class with no parameters.
        public Information() { }
    }  
}
