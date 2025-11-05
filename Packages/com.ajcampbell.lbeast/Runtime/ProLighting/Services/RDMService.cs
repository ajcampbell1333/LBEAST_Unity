// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LBEAST.ProLighting
{
    /// <summary>Lightweight RDM management service (discovery cache + lifecycle)</summary>
    public class RDMService
    {
        public event Action<LBEASTDiscoveredFixture> OnDiscoveredEvent;
        public event Action<int> OnWentOfflineEvent;
        public event Action<int> OnCameOnlineEvent;

        private readonly Dictionary<string, LBEASTDiscoveredFixture> discovered = new Dictionary<string, LBEASTDiscoveredFixture>();
        private float pollInterval = 0.5f;
        private float accumulated = 0f;

        public void Initialize(float pollIntervalSeconds)
        {
            pollInterval = Mathf.Max(0.1f, pollIntervalSeconds);
            accumulated = 0f;
        }

        public void Tick(float deltaTime)
        {
            accumulated += deltaTime;
            if (accumulated >= pollInterval)
            {
                accumulated = 0f;
                // Polling is driven externally (controller) for now; nothing here.
            }
        }

        public bool AddOrUpdate(LBEASTDiscoveredFixture fixture)
        {
            var isNew = !discovered.ContainsKey(fixture.RDMUID);
            var entry = discovered.ContainsKey(fixture.RDMUID) ? discovered[fixture.RDMUID] : new LBEASTDiscoveredFixture();
            entry = fixture;
            entry.LastSeenTimestamp = DateTime.Now;
            discovered[fixture.RDMUID] = entry;
            if (isNew)
                OnDiscoveredEvent?.Invoke(entry);
            return isNew;
        }

        public bool TryGet(string rdmUID, out LBEASTDiscoveredFixture fixture)
        {
            if (discovered.TryGetValue(rdmUID, out var found))
            {
                fixture = found;
                return true;
            }
            fixture = null;
            return false;
        }

        public LBEASTDiscoveredFixture FindMutable(string rdmUID) => discovered.TryGetValue(rdmUID, out var fixture) ? fixture : null;

        public List<LBEASTDiscoveredFixture> GetAll() => discovered.Values.ToList();

        public void MarkOnline(string rdmUID, int virtualFixtureID)
        {
            if (!discovered.TryGetValue(rdmUID, out var p)) return;
            var wasOffline = !p.IsOnline;
            p.IsOnline = true;
            p.LastSeenTimestamp = DateTime.Now;
            discovered[rdmUID] = p;
            if (wasOffline)
                OnCameOnlineEvent?.Invoke(virtualFixtureID);
        }

        public void MarkOffline(string rdmUID, int virtualFixtureID)
        {
            if (!discovered.TryGetValue(rdmUID, out var p)) return;
            if (p.IsOnline)
            {
                p.IsOnline = false;
                discovered[rdmUID] = p;
                OnWentOfflineEvent?.Invoke(virtualFixtureID);
            }
        }

        public void Prune(float offlineThresholdSeconds, float removeThresholdSeconds, List<int> outWentOfflineVirtualIDs, List<string> outRemovedUIDs, Dictionary<string, int> rdmToVirtual)
        {
            var now = DateTime.Now;
            var keys = discovered.Keys.ToList();
            foreach (var uid in keys)
            {
                var f = discovered[uid];
                var since = (now - f.LastSeenTimestamp).TotalSeconds;
                if (since > offlineThresholdSeconds && f.IsOnline)
                {
                    f.IsOnline = false;
                    discovered[uid] = f;
                    if (rdmToVirtual.TryGetValue(uid, out var v))
                    {
                        outWentOfflineVirtualIDs.Add(v);
                        OnWentOfflineEvent?.Invoke(v);
                    }
                }
                if (since > removeThresholdSeconds)
                    outRemovedUIDs.Add(uid);
            }
            foreach (var uid in outRemovedUIDs)
                discovered.Remove(uid);
        }
    }
}

