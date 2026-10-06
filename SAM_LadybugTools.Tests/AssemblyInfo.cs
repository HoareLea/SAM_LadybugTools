// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors

// HoneybeeSchema.Helper.EnergyLibrary downloads its standards (energy_default.json) into a
// single shared %TEMP% path on first use. With xunit's default parallel collections, tests
// that touch the library race on that download on machines with a cold cache (fresh CI
// runners) and fail with IOException. The suite runs in ~1 s, so serialising collections
// costs nothing and makes the first-touch download deterministic.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]
