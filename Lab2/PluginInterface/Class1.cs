using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
    };

    public interface ICrypography : IPlugin
    {
        string ChyperName { get; }
        byte[] Encrypt(byte[] arr);
        byte[] Decrypt(byte[] arr);
    };

    public enum PluginType
    {
        Cryptography,
        Unknown
    };

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginAttribute : Attribute
    {
        private PluginType _Type;
        public PluginAttribute(PluginType T) { _Type = T; }
        public PluginType Type { get { return _Type; } }
    };

   
}
