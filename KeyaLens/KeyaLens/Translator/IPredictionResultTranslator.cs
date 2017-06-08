using KeyaLens.DataModel;

namespace KeyaLens.Translator
{
    public interface IPredictionResultTranslator
    {
        PredictionResultModel Translate(string Name);
    }
}