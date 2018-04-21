using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace TextReplacer.Components
{
    /// <summary>
    /// Replaces "Tridion" in the input string to "SDL Tridion".
    /// </summary>
    public class Replacer
    {
        /// <summary>
        /// Replaces "Tridion" in the input string to "SDL Tridion".
        /// </summary>
        /// <param name="input">Input XML string</param>
        /// <returns>The replaced </returns>
        public string Process(string input)
        {
            var args = new ReplaceArgs { Input = input };
            var res = Process(args);
            return res.Output;
        }

        /// <summary>
        /// Replaces "Tridion" in the input <see cref="ReplaceArgs.Input"/> to "SDL Tridion".
        /// </summary>
        /// <returns><see cref="ReplaceResult"/></returns>
        public ReplaceResult Process(ReplaceArgs args)
        {
            var res = new ReplaceResult
            {
                Output = args.Input
            };
            try
            {
                XPathNavigator navigator;
                XmlDocument xmlDocument;
                try
                {
                    xmlDocument = LoadXml(args.Input);
                    navigator = xmlDocument.CreateNavigator();
                }
                catch (Exception ex)
                {
                    res.Error = $"Invalid xml ({ex.Message}).";
                    return res;
                }
 
                ProcessElements(navigator, (XPathNavigator node) =>
                {
                    if (!new[] { XPathNodeType.Text, XPathNodeType.Attribute }.Contains(node.NodeType))
                        return;
                    var newString = Regex.Replace(node.Value, $"\\w*(?<!SDL )Trisoft", "SDL Trisoft");
                    if (newString != node.Value)
                    {
                        res.IsProcessed = true;
                        node.SetValue(newString);
                    }
                });

                if (res.IsProcessed)
                    res.Output = xmlDocument.OuterXml;
            }
            catch (Exception ex)
            {
                res.Error = $"Unhandled error ({ex.Message})";
            }

            return res;
        }

        /// <summary>
        /// Loads the xml document.
        /// </summary>
        private XmlDocument LoadXml(string input)
        {
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };
            xmlDocument.LoadXml(input);
            return xmlDocument;
        }

        /// <summary>
        /// Processes single node calling <see cref="action"/> on it, its children and attributes.
        /// </summary>
        /// <param name="node">Node to process.</param>
        /// <param name="action">Action to execute.</param>
        private void ProcessElements(XPathNavigator node, Action<XPathNavigator> action)
        {
            if (node.HasAttributes)
            {
                var attrNavigator = node.Clone();
                attrNavigator.MoveToFirstAttribute();
                do
                {
                    action(attrNavigator);
                } while (attrNavigator.MoveToNextAttribute());
            }

            if (node.HasChildren)
            {
                node.MoveToFirstChild();
                do
                {
                    action(node);
                    ProcessElements(node, action);
                } while (node.MoveToNext());
            }
        }
    }
}