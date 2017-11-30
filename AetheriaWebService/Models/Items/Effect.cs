using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AetheriaWebService.Models.CharacterStats;

namespace AetheriaWebService.Models
{
    public class Effect
    {
        public enum EModifyType
        {
            None = 0,
            Add = 10,
            Sub = 20,
            Mul = 30,
        }
        public enum ETimingType
        {
            None = 0,
            Duration = 10,
            Immediate = 20,
            Permanent = 30,
        }
        public Guid EffectId { get; set; }
        public string EffectName { get; set; }
        public EStatType AffectedStat { get; set; }
        public double EffectValue { get; set; }
        public EModifyType ModifyType { get; set; }
        public ETimingType TimingType { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EffectStarted { get; set; }
    }
}
