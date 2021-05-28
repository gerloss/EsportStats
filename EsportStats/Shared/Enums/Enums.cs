using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EsportStats.Shared.Enums
{
    public enum Metric
    {
        [Display(Name = "Please select a metric")]
        PleaseSelect = 0,
        [Display(Name = "APM", ShortName = "actions_per_min")]
        actions_per_min,
        [Display(Name = "Assists", ShortName = "assists")]
        assists,
        [Display(Name = "Biggest Comeback", ShortName = "comeback")]
        comeback,
        [Display(Name = "Courier Kills", ShortName = "courier_kills")]
        courier_kills,
        [Display(Name = "Deaths", ShortName = "deaths")]
        deaths,
        [Display(Name = "Denies", ShortName = "denies")]
        denies,
        [Display(Name = "Gems Purchased", ShortName = "purchase_gem")]
        purchase_gem,
        [Display(Name = "GPM", ShortName = "gold_per_min")]
        gold_per_min,
        [Display(Name = "Hero Damage", ShortName = "hero_damage")]
        hero_damage,
        [Display(Name = "Hero Healing", ShortName = "hero_healing")]
        hero_healing,
        [Display(Name = "KDA", ShortName = "kda")]
        kda,
        [Display(Name = "Kills", ShortName = "kills")]
        kills,
        [Display(Name = "Lane Efficiency Pct", ShortName = "lane_efficiency_pct")]
        lane_efficiency_pct,
        [Display(Name = "Last Hits", ShortName = "last_hits")]
        last_hits,
        [Display(Name = "Biggest Loss", ShortName = "loss")]        
        loss,
        [Display(Name = "Match Duration", ShortName = "duration")]
        duration,
        [Display(Name = "Neutral Kills", ShortName = "neutral_kills")]
        neutral_kills,
        [Display(Name = "Observer Wards Purchased", ShortName = "purchase_ward_observer")]
        purchase_ward_observer,
        [Display(Name = "Amount of Pings", ShortName = "pings")]
        pings,
        [Display(Name = "Rapiers Purchased", ShortName = "purchase_rapier")]
        purchase_rapier,
        [Display(Name = "Sentry Wards Purchased", ShortName = "purchase_ward_sentry")]        
        purchase_ward_sentry,
        [Display(Name = "Biggest Stomp", ShortName = "stomp")]
        stomp,
        [Display(Name = "Stuns", ShortName = "stuns")]
        stuns,
        [Display(Name = "Tower Damage", ShortName = "tower_damage")]
        tower_damage,
        [Display(Name = "Tower Kills", ShortName = "tower_kills")]
        tower_kills,
        [Display(Name = "TP Scrolls Purchased", ShortName = "purchase_tpscroll")]
        purchase_tpscroll,
        [Display(Name = "XPM", ShortName = "xp_per_min")]
        xp_per_min
    }

    public enum Hero
    {
        [Display(Name = "Please select a hero...")]
        PleaseSelect = 0,
        [Display(Name = "Anti-Mage", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/antimage.png")]
        Antimage = 1,
        [Display(Name = "Axe", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/axe.png")]
        Axe = 2,
        [Display(Name = "Bane", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/bane.png")]
        Bane = 3,
        [Display(Name = "Bloodseeker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/bloodseeker.png")]
        Bloodseeker = 4,
        [Display(Name = "Crystal Maiden", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/crystal_maiden.png")]
        CrystalMaiden = 5,
        [Display(Name = "Drow Ranger", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/drow_ranger.png")]
        DrowRanger = 6,
        [Display(Name = "Earthshaker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/earthshaker.png")]
        Earthshaker = 7,
        [Display(Name = "Juggernaut", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/juggernaut.png")]
        Juggernaut = 8,
        [Display(Name = "Mirana", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/mirana.png")]
        Mirana = 9,
        [Display(Name = "Morphling", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/morphling.png")]
        Morphling = 10,
        [Display(Name = "Shadow Fiend", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/nevermore.png")]
        ShadowFiend = 11,
        [Display(Name = "Phantom Lancer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/phantom_lancer.png")]
        PhantomLancer = 12,
        [Display(Name = "Puck", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/puck.png")]
        Puck = 13,
        [Display(Name = "Pudge", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/pudge.png")]
        Pudge = 14,
        [Display(Name = "Razor", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/razor.png")]
        Razor = 15,
        [Display(Name = "Sand King", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/sand_king.png")]
        SandKing = 16,
        [Display(Name = "Storm Spirit", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/storm_spirit.png")]
        StormSpirit = 17,
        [Display(Name = "Sven", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/sven.png")]
        Sven = 18,
        [Display(Name = "Tiny", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/tiny.png")]
        Tiny = 19,
        [Display(Name = "Vengeful Spirit", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/vengefulspirit.png")]
        VengefulSpirit = 20,
        [Display(Name = "Windranger", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/windrunner.png")]
        Windranger = 21,
        [Display(Name = "Zeus", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/zuus.png")]
        Zeus = 22,
        [Display(Name = "Kunkka", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/kunkka.png")]
        Kunkka = 23,
        [Display(Name = "Lina", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/lina.png")]
        Lina = 25,
        [Display(Name = "Lion", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/lion.png")]
        Lion = 26,
        [Display(Name = "Shadow Shaman", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/shadow_shaman.png")]
        ShadowShaman = 27,
        [Display(Name = "Slardar", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/slardar.png")]
        Slardar = 28,
        [Display(Name = "Tidehunter", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/tidehunter.png")]
        Tidehunter = 29,
        [Display(Name = "Witch Doctor", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/witch_doctor.png")]
        WitchDoctor = 30,
        [Display(Name = "Lich", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/lich.png")]
        Lich = 31,
        [Display(Name = "Riki", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/riki.png")]
        Riki = 32,
        [Display(Name = "Enigma", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/enigma.png")]
        Enigma = 33,
        [Display(Name = "Tinker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/tinker.png")]
        Tinker = 34,
        [Display(Name = "Sniper", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/sniper.png")]
        Sniper = 35,
        [Display(Name = "Necrophos", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/necrolyte.png")]
        Necrophos = 36,
        [Display(Name = "Warlock", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/warlock.png")]
        Warlock = 37,
        [Display(Name = "Beastmaster", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/beastmaster.png")]
        Beastmaster = 38,
        [Display(Name = "Queen of Pain", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/queenofpain.png")]
        QueenofPain = 39,
        [Display(Name = "Venomancer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/venomancer.png")]
        Venomancer = 40,
        [Display(Name = "Faceless Void", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/faceless_void.png")]
        FacelessVoid = 41,
        [Display(Name = "Wraith King", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/skeleton_king.png")]
        WraithKing = 42,
        [Display(Name = "Death Prophet", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/death_prophet.png")]
        DeathProphet = 43,
        [Display(Name = "Phantom Assassin", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/phantom_assassin.png")]
        PhantomAssassin = 44,
        [Display(Name = "Pugna", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/pugna.png")]
        Pugna = 45,
        [Display(Name = "Templar Assassin", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/templar_assassin.png")]
        TemplarAssassin = 46,
        [Display(Name = "Viper", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/viper.png")]
        Viper = 47,
        [Display(Name = "Luna", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/luna.png")]
        Luna = 48,
        [Display(Name = "Dragon Knight", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/dragon_knight.png")]
        DragonKnight = 49,
        [Display(Name = "Dazzle", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/dazzle.png")]
        Dazzle = 50,
        [Display(Name = "Clockwerk", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/rattletrap.png")]
        Clockwerk = 51,
        [Display(Name = "Leshrac", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/leshrac.png")]
        Leshrac = 52,
        [Display(Name = "Nature's Prophet", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/furion.png")]
        NaturesProphet = 53,
        [Display(Name = "Lifestealer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/life_stealer.png")]
        Lifestealer = 54,
        [Display(Name = "Dark Seer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/dark_seer.png")]
        DarkSeer = 55,
        [Display(Name = "Clinkz", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/clinkz.png")]
        Clinkz = 56,
        [Display(Name = "Omniknight", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/omniknight.png")]
        Omniknight = 57,
        [Display(Name = "Enchantress", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/enchantress.png")]
        Enchantress = 58,
        [Display(Name = "Huskar", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/huskar.png")]
        Huskar = 59,
        [Display(Name = "Night Stalker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/night_stalker.png")]
        NightStalker = 60,
        [Display(Name = "Broodmother", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/broodmother.png")]
        Broodmother = 61,
        [Display(Name = "Bounty Hunter", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/bounty_hunter.png")]
        BountyHunter = 62,
        [Display(Name = "Weaver", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/weaver.png")]
        Weaver = 63,
        [Display(Name = "Jakiro", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/jakiro.png")]
        Jakiro = 64,
        [Display(Name = "Batrider", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/batrider.png")]
        Batrider = 65,
        [Display(Name = "Chen", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/chen.png")]
        Chen = 66,
        [Display(Name = "Spectre", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/spectre.png")]
        Spectre = 67,
        [Display(Name = "Ancient Apparition", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/ancient_apparition.png")]
        AncientApparition = 68,
        [Display(Name = "Doom", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/doom_bringer.png")]
        Doom = 69,
        [Display(Name = "Ursa", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/ursa.png")]
        Ursa = 70,
        [Display(Name = "Spirit Breaker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/spirit_breaker.png")]
        SpiritBreaker = 71,
        [Display(Name = "Gyrocopter", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/gyrocopter.png")]
        Gyrocopter = 72,
        [Display(Name = "Alchemist", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/alchemist.png")]
        Alchemist = 73,
        [Display(Name = "Invoker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/invoker.png")]
        Invoker = 74,
        [Display(Name = "Silencer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/silencer.png")]
        Silencer = 75,
        [Display(Name = "Outworld Destroyer", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/obsidian_destroyer.png")]
        OutworldDestroyer = 76,
        [Display(Name = "Lycan", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/lycan.png")]
        Lycan = 77,
        [Display(Name = "Brewmaster", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/brewmaster.png")]
        Brewmaster = 78,
        [Display(Name = "Shadow Demon", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/shadow_demon.png")]
        ShadowDemon = 79,
        [Display(Name = "Lone Druid", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/lone_druid.png")]
        LoneDruid = 80,
        [Display(Name = "Chaos Knight", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/chaos_knight.png")]
        ChaosKnight = 81,
        [Display(Name = "Meepo", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/meepo.png")]
        Meepo = 82,
        [Display(Name = "Treant Protector", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/treant.png")]
        TreantProtector = 83,
        [Display(Name = "Ogre Magi", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/ogre_magi.png")]
        OgreMagi = 84,
        [Display(Name = "Undying", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/undying.png")]
        Undying = 85,
        [Display(Name = "Rubick", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/rubick.png")]
        Rubick = 86,
        [Display(Name = "Disruptor", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/disruptor.png")]
        Disruptor = 87,
        [Display(Name = "Nyx Assassin", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/nyx_assassin.png")]
        NyxAssassin = 88,
        [Display(Name = "Naga Siren", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/naga_siren.png")]
        NagaSiren = 89,
        [Display(Name = "Keeper of the Light", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/keeper_of_the_light.png")]
        KeeperoftheLight = 90,
        [Display(Name = "Io", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/wisp.png")]
        Io = 91,
        [Display(Name = "Visage", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/visage.png")]
        Visage = 92,
        [Display(Name = "Slark", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/slark.png")]
        Slark = 93,
        [Display(Name = "Medusa", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/medusa.png")]
        Medusa = 94,
        [Display(Name = "Troll Warlord", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/troll_warlord.png")]
        TrollWarlord = 95,
        [Display(Name = "Centaur Warrunner", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/centaur.png")]
        CentaurWarrunner = 96,
        [Display(Name = "Magnus", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/magnataur.png")]
        Magnus = 97,
        [Display(Name = "Timbersaw", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/shredder.png")]
        Timbersaw = 98,
        [Display(Name = "Bristleback", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/bristleback.png")]
        Bristleback = 99,
        [Display(Name = "Tusk", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/tusk.png")]
        Tusk = 100,
        [Display(Name = "Skywrath Mage", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/skywrath_mage.png")]
        SkywrathMage = 101,
        [Display(Name = "Abaddon", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/abaddon.png")]
        Abaddon = 102,
        [Display(Name = "Elder Titan", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/elder_titan.png")]
        ElderTitan = 103,
        [Display(Name = "Legion Commander", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/legion_commander.png")]
        LegionCommander = 104,
        [Display(Name = "Techies", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/techies.png")]
        Techies = 105,
        [Display(Name = "Ember Spirit", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/ember_spirit.png")]
        EmberSpirit = 106,
        [Display(Name = "Earth Spirit", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/earth_spirit.png")]
        EarthSpirit = 107,
        [Display(Name = "Underlord", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/abyssal_underlord.png")]
        Underlord = 108,
        [Display(Name = "Terrorblade", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/terrorblade.png")]
        Terrorblade = 109,
        [Display(Name = "Phoenix", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/phoenix.png")]
        Phoenix = 110,
        [Display(Name = "Oracle", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/oracle.png")]
        Oracle = 111,
        [Display(Name = "Winter Wyvern", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/winter_wyvern.png")]
        WinterWyvern = 112,
        [Display(Name = "Arc Warden", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/arc_warden.png")]
        ArcWarden = 113,
        [Display(Name = "Monkey King", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/monkey_king.png")]
        MonkeyKing = 114,
        [Display(Name = "Dark Willow", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/dark_willow.png")]
        DarkWillow = 119,
        [Display(Name = "Pangolier", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/pangolier.png")]
        Pangolier = 120,
        [Display(Name = "Grimstroke", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/grimstroke.png")]
        Grimstroke = 121,
        [Display(Name = "Hoodwink", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/hoodwink.png")]
        Hoodwink = 123,
        [Display(Name = "Void Spirit", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/void_spirit.png")]
        VoidSpirit = 126,
        [Display(Name = "Snapfire", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/snapfire.png")]
        Snapfire = 128,
        [Display(Name = "Mars", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/mars.png")]
        Mars = 129,
        [Display(Name = "Dawnbreaker", Description = "https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/dawnbreaker.png")]
        Dawnbreaker = 135

    }
}
