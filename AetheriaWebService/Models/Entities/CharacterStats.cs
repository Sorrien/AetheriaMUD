using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Models
{
    public class CharacterStats
    {
        public enum EStatType
        {
            None = 0,
            Health = 10,
            MaxHealth = 20,
            ActionPoints = 30,
            MaxActionPoints = 40,
        }

        public Guid CharacterStatsId { get; set; }
        public double CurrentHealthPoints { get; set; }
        public double CurrentActionPoints { get; set; }

        public double MaximumHealthPoints
        {
            get
            {
                var maxhp = 10d;

                maxhp = CalculateAffectedStat(maxhp, EStatType.MaxHealth);

                return maxhp;
            }
        }

        public double MaximumActionPoints
        {
            get
            {
                var maxap = 10d;

                maxap = CalculateAffectedStat(maxap, EStatType.MaxActionPoints);

                return maxap;
            }
        }

        private double CalculateAffectedStat(double baseValue, EStatType statType)
        {
            var modifiedValue = baseValue;
            var maxapEffects = EffectsForStatType(statType);
            foreach (var effect in maxapEffects)
            {
                switch (effect.ModifyType)
                {
                    case Effect.EModifyType.Add:
                        modifiedValue += effect.EffectValue;
                        break;
                    case Effect.EModifyType.Sub:
                        modifiedValue -= effect.EffectValue;
                        break;
                    case Effect.EModifyType.Mul:
                        modifiedValue += baseValue * effect.EffectValue;
                        break;
                }
            }
            return modifiedValue;
        }

        public int Level { get; set; }

        public List<Effect> Effects { get; set; }

        private List<Effect> EffectsForStatType(EStatType statType)
        {
            return Effects.Where(x => x.AffectedStat == statType &&
            (x.TimingType == Effect.ETimingType.Duration && ((DateTime.Now - x.EffectStarted) <= x.Duration
            || x.TimingType == Effect.ETimingType.Permanent)))
            .ToList();
        }
    }
}
