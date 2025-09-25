using Content.Shared.Containers.ItemSlots;
using Content.Shared.Coordinates;
using Robust.Shared.Audio;
using Content.Shared.Audio;
using Robust.Shared.Network;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio.Systems;
using Content.Shared.Popups;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Timing;

namespace Content.Shared._Arcadis.DeepSleep;

public sealed class DeepSleepSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ILogManager _logMan = default!;
    [Dependency] private readonly INetManager _netMan = default!;

    private ISawmill _sawmill = default!;
    public override void Initialize()
    {
        base.Initialize();

        // SubscribeLocalEvent<DeepSleepSleepingComponent, ExaminedEvent>(OnDeepSleepAction);
        // SubscribeLocalEvent()

        _sawmill = _logMan.GetSawmill("snooze");
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DeepSleepSleepingComponent>();

        if (!_timing.IsFirstTimePredicted)
            return;

        while (query.MoveNext(out var uid, out var comp))
        {
            if (!TryComp<DeepSleepShaderComponent>(uid, out var shadercomp))
                EnsureComp<DeepSleepShaderComponent>(uid, out shadercomp);

            if (shadercomp.SleepProgression >= 1f)
            {
                shadercomp.SleepProgression = 1f;
                continue;
            }

            _sawmill.Debug((comp.SleepProgressionSpeed / _timing.TickRate).ToString());

            _sawmill.Debug(shadercomp.SleepProgression.ToString());

            shadercomp.SleepProgression += (comp.SleepProgressionSpeed / _timing.TickRate);
            if (shadercomp.SleepProgression <= 0)
            {
                RemComp<DeepSleepShaderComponent>(uid);
                RemComp<DeepSleepSleepingComponent>(uid);
            }
        }
    }
}
