using Lax.Data.SharePoint.Lists.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    [PublicAPI]
    internal interface IProcessor<in TIn, out TOut> {

        TOut Process(TIn input);

    }

}