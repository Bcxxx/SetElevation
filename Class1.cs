using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetElevation
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Get application and document objects
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            IList<Reference> pickedObjs = uidoc.Selection.PickObjects(ObjectType.Element, "Select Elements");
            List<ElementId> ids = (from Reference r in pickedObjs select r.ElementId).ToList();
            using (Transaction tx = new Transaction(doc))
            {
                StringBuilder sb = new StringBuilder();
                tx.Start("transaction");
                if (pickedObjs != null && pickedObjs.Count > 0)
                {
                    foreach (ElementId eid in ids)
                    {
                        Element e = doc.GetElement(eid);
                        if(e.GetParameters("OK") == null && e.GetParameters("UK") == null)
                        {

                        }
                        sb.Append("\n" + e.Name);
                    }
                    TaskDialog.Show("Elements picked: ", sb.ToString());
                }
                tx.Commit();  
            }
            return Result.Succeeded;
        }

        //***GetParameterValue()***
        public string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.None:
                    return parameter.AsValueString();
                case StorageType.Integer:
                    //get value with unit, AsInteger() can get value without unit
                    return parameter.AsValueString();
                case StorageType.Double:
                    //get value with unit, AsDouble() can get value without unit
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                case StorageType.ElementId:
                    return parameter.AsElementId().IntegerValue.ToString();
                default:
                    return "";
            }
        }
    }
}
 