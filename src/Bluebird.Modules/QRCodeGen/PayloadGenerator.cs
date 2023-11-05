#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace Bluebird.Modules.QRCodeGen;

public static class PayloadGenerator
{
    public abstract class Payload
    {
        public virtual int Version { get { return -1; } }
        public virtual QRCodeGenerator.ECCLevel EccLevel { get { return QRCodeGenerator.ECCLevel.M; } }
        public virtual QRCodeGenerator.EciMode EciMode { get { return QRCodeGenerator.EciMode.Default; } }
        public abstract override string ToString();
    }

    public class Url : Payload
    {
        private readonly string url;

        /// <summary>
        /// Generates a link. If not given, http/https protocol will be added.
        /// </summary>
        /// <param name="url">Link url target</param>
        public Url(string url)
        {
            this.url = url;
        }

        public override string ToString()
        {
            return (!this.url.StartsWith("http") ? "http://" + this.url : this.url);
        }
    }
}
