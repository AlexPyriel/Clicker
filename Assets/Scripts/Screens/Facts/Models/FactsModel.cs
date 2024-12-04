using System.Collections.Generic;
using UniRx;

namespace Screens.Facts.Models
{
    public class FactsModel
    {
        public ReactiveProperty<List<FactModel>> Breeds { get; } = new(new List<FactModel>());

        public void Update(List<FactModel> breeds)
        {
            Breeds.Value = breeds;
        }
    }
}