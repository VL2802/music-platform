using SharedLibraryML.CollectionSection;
using SharedLibraryML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace ML_LogicLayer.Validators
{
    public static class CollectionValidator
    {
       

        public static void Validate(Collection collection)
        {
           

            if (collection.GetMedias().Count == 0)
            {
                throw new ArgumentException("The collection should have at least one media");

            }
           
        }

       

      
                
         
    }

}
