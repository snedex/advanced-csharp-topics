using System.Dynamic;
using System.Xml.Linq;

namespace CSharpTopics.Section3
{
    public class DynamicXmlNode : DynamicObject
    {
        private XElement Node;

        internal DynamicXmlNode(XElement node)
        {
            this.Node = node;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            //try get the element first
            var element = Node.Element(binder.Name);

            if (element != null)
            {
                //Return a wrapped object se we can continue traversal
                result = new DynamicXmlNode(element);
                return true;
            }
            else
            {
                //Otherwise try to get the attribute
                var attribute = Node.Attribute(binder.Name);
                if (attribute != null)
                {
                    result = attribute.Value;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }
    }
}