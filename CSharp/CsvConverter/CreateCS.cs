using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace CsvConverter
{
    public class CreateCS
    {
        CodeCompileUnit targetUnit;
        CodeTypeDeclaration targetClass;
        public CreateCS(string name)
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace();
            //samples.Imports.Add(new CodeNamespaceImport("System"));
            targetClass = new CodeTypeDeclaration(name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public;
            targetClass.CustomAttributes.Add(new CodeAttributeDeclaration("System.Serializable"));
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);
        }

        public void AddFields(string type, string name)
        {
            CodeMemberField ValueField = new CodeMemberField();
            ValueField.Attributes = MemberAttributes.Public;
            ValueField.Name = name;
            if (type == "string")
            {
                ValueField.Type = new CodeTypeReference(typeof(System.String));
            }
            else if (type == "int")
            {
                ValueField.Type = new CodeTypeReference(typeof(System.Int32));
            }
            else if (type == "float")
            {
                ValueField.Type = new CodeTypeReference(typeof(System.Single));
            }
            else if (type == "bool")
            {
                ValueField.Type = new CodeTypeReference(typeof(System.Boolean));
            }
            targetClass.Members.Add(ValueField);
        }

        public void GenerateCSharpCode(string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }
    }
}