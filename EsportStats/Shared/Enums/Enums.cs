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
        [Display(Name = "Anti-Mage", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/antimage_sb.png")]
        Antimage = 1,
        [Display(Name = "Axe", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/axe_sb.png")]
        Axe = 2,
        [Display(Name = "Bane", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/bane_sb.png")]
        Bane = 3,
        [Display(Name = "Bloodseeker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/bloodseeker_sb.png")]
        Bloodseeker = 4,
        [Display(Name = "Crystal Maiden", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/crystal_maiden_sb.png")]
        CrystalMaiden = 5,
        [Display(Name = "Drow Ranger", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/drow_ranger_sb.png")]
        DrowRanger = 6,
        [Display(Name = "Earthshaker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/earthshaker_sb.png")]
        Earthshaker = 7,
        [Display(Name = "Juggernaut", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/juggernaut_sb.png")]
        Juggernaut = 8,
        [Display(Name = "Mirana", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/mirana_sb.png")]
        Mirana = 9,
        [Display(Name = "Morphling", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/morphling_sb.png")]
        Morphling = 10,
        [Display(Name = "Shadow Fiend", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/nevermore_sb.png")]
        ShadowFiend = 11,
        [Display(Name = "Phantom Lancer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/phantom_lancer_sb.png")]
        PhantomLancer = 12,
        [Display(Name = "Puck", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/puck_sb.png")]
        Puck = 13,
        [Display(Name = "Pudge", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/pudge_sb.png")]
        Pudge = 14,
        [Display(Name = "Razor", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/razor_sb.png")]
        Razor = 15,
        [Display(Name = "Sand King", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/sand_king_sb.png")]
        SandKing = 16,
        [Display(Name = "Storm Spirit", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/storm_spirit_sb.png")]
        StormSpirit = 17,
        [Display(Name = "Sven", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/sven_sb.png")]
        Sven = 18,
        [Display(Name = "Tiny", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/tiny_sb.png")]
        Tiny = 19,
        [Display(Name = "Vengeful Spirit", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/vengefulspirit_sb.png")]
        VengefulSpirit = 20,
        [Display(Name = "Windranger", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/windrunner_sb.png")]
        Windranger = 21,
        [Display(Name = "Zeus", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/zuus_sb.png")]
        Zeus = 22,
        [Display(Name = "Kunkka", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/kunkka_sb.png")]
        Kunkka = 23,
        [Display(Name = "Lina", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/lina_sb.png")]
        Lina = 25,
        [Display(Name = "Lion", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/lion_sb.png")]
        Lion = 26,
        [Display(Name = "Shadow Shaman", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/shadow_shaman_sb.png")]
        ShadowShaman = 27,
        [Display(Name = "Slardar", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/slardar_sb.png")]
        Slardar = 28,
        [Display(Name = "Tidehunter", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/tidehunter_sb.png")]
        Tidehunter = 29,
        [Display(Name = "Witch Doctor", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/witch_doctor_sb.png")]
        WitchDoctor = 30,
        [Display(Name = "Lich", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/lich_sb.png")]
        Lich = 31,
        [Display(Name = "Riki", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/riki_sb.png")]
        Riki = 32,
        [Display(Name = "Enigma", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/enigma_sb.png")]
        Enigma = 33,
        [Display(Name = "Tinker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/tinker_sb.png")]
        Tinker = 34,
        [Display(Name = "Sniper", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/sniper_sb.png")]
        Sniper = 35,
        [Display(Name = "Necrophos", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/necrolyte_sb.png")]
        Necrophos = 36,
        [Display(Name = "Warlock", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/warlock_sb.png")]
        Warlock = 37,
        [Display(Name = "Beastmaster", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/beastmaster_sb.png")]
        Beastmaster = 38,
        [Display(Name = "Queen of Pain", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/queenofpain_sb.png")]
        QueenofPain = 39,
        [Display(Name = "Venomancer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/venomancer_sb.png")]
        Venomancer = 40,
        [Display(Name = "Faceless Void", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/faceless_void_sb.png")]
        FacelessVoid = 41,
        [Display(Name = "Wraith King", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/skeleton_king_sb.png")]
        WraithKing = 42,
        [Display(Name = "Death Prophet", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/death_prophet_sb.png")]
        DeathProphet = 43,
        [Display(Name = "Phantom Assassin", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/phantom_assassin_sb.png")]
        PhantomAssassin = 44,
        [Display(Name = "Pugna", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/pugna_sb.png")]
        Pugna = 45,
        [Display(Name = "Templar Assassin", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/templar_assassin_sb.png")]
        TemplarAssassin = 46,
        [Display(Name = "Viper", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/viper_sb.png")]
        Viper = 47,
        [Display(Name = "Luna", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/luna_sb.png")]
        Luna = 48,
        [Display(Name = "Dragon Knight", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/dragon_knight_sb.png")]
        DragonKnight = 49,
        [Display(Name = "Dazzle", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/dazzle_sb.png")]
        Dazzle = 50,
        [Display(Name = "Clockwerk", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/rattletrap_sb.png")]
        Clockwerk = 51,
        [Display(Name = "Leshrac", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/leshrac_sb.png")]
        Leshrac = 52,
        [Display(Name = "Nature's Prophet", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/furion_sb.png")]
        NaturesProphet = 53,
        [Display(Name = "Lifestealer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/life_stealer_sb.png")]
        Lifestealer = 54,
        [Display(Name = "Dark Seer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/dark_seer_sb.png")]
        DarkSeer = 55,
        [Display(Name = "Clinkz", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/clinkz_sb.png")]
        Clinkz = 56,
        [Display(Name = "Omniknight", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/omniknight_sb.png")]
        Omniknight = 57,
        [Display(Name = "Enchantress", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/enchantress_sb.png")]
        Enchantress = 58,
        [Display(Name = "Huskar", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/huskar_sb.png")]
        Huskar = 59,
        [Display(Name = "Night Stalker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/night_stalker_sb.png")]
        NightStalker = 60,
        [Display(Name = "Broodmother", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/broodmother_sb.png")]
        Broodmother = 61,
        [Display(Name = "Bounty Hunter", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/bounty_hunter_sb.png")]
        BountyHunter = 62,
        [Display(Name = "Weaver", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/weaver_sb.png")]
        Weaver = 63,
        [Display(Name = "Jakiro", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/jakiro_sb.png")]
        Jakiro = 64,
        [Display(Name = "Batrider", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/batrider_sb.png")]
        Batrider = 65,
        [Display(Name = "Chen", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/chen_sb.png")]
        Chen = 66,
        [Display(Name = "Spectre", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/spectre_sb.png")]
        Spectre = 67,
        [Display(Name = "Ancient Apparition", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/ancient_apparition_sb.png")]
        AncientApparition = 68,
        [Display(Name = "Doom", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/doom_bringer_sb.png")]
        Doom = 69,
        [Display(Name = "Ursa", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/ursa_sb.png")]
        Ursa = 70,
        [Display(Name = "Spirit Breaker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/spirit_breaker_sb.png")]
        SpiritBreaker = 71,
        [Display(Name = "Gyrocopter", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/gyrocopter_sb.png")]
        Gyrocopter = 72,
        [Display(Name = "Alchemist", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/alchemist_sb.png")]
        Alchemist = 73,
        [Display(Name = "Invoker", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/invoker_sb.png")]
        Invoker = 74,
        [Display(Name = "Silencer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/silencer_sb.png")]
        Silencer = 75,
        [Display(Name = "Outworld Destroyer", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/obsidian_destroyer_sb.png")]
        OutworldDestroyer = 76,
        [Display(Name = "Lycan", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/lycan_sb.png")]
        Lycan = 77,
        [Display(Name = "Brewmaster", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/brewmaster_sb.png")]
        Brewmaster = 78,
        [Display(Name = "Shadow Demon", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/shadow_demon_sb.png")]
        ShadowDemon = 79,
        [Display(Name = "Lone Druid", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/lone_druid_sb.png")]
        LoneDruid = 80,
        [Display(Name = "Chaos Knight", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/chaos_knight_sb.png")]
        ChaosKnight = 81,
        [Display(Name = "Meepo", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/meepo_sb.png")]
        Meepo = 82,
        [Display(Name = "Treant Protector", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/treant_sb.png")]
        TreantProtector = 83,
        [Display(Name = "Ogre Magi", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/ogre_magi_sb.png")]
        OgreMagi = 84,
        [Display(Name = "Undying", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/undying_sb.png")]
        Undying = 85,
        [Display(Name = "Rubick", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/rubick_sb.png")]
        Rubick = 86,
        [Display(Name = "Disruptor", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/disruptor_sb.png")]
        Disruptor = 87,
        [Display(Name = "Nyx Assassin", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/nyx_assassin_sb.png")]
        NyxAssassin = 88,
        [Display(Name = "Naga Siren", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/naga_siren_sb.png")]
        NagaSiren = 89,
        [Display(Name = "Keeper of the Light", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/keeper_of_the_light_sb.png")]
        KeeperoftheLight = 90,
        [Display(Name = "Io", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/wisp_sb.png")]
        Io = 91,
        [Display(Name = "Visage", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/visage_sb.png")]
        Visage = 92,
        [Display(Name = "Slark", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/slark_sb.png")]
        Slark = 93,
        [Display(Name = "Medusa", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/medusa_sb.png")]
        Medusa = 94,
        [Display(Name = "Troll Warlord", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/troll_warlord_sb.png")]
        TrollWarlord = 95,
        [Display(Name = "Centaur Warrunner", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/centaur_sb.png")]
        CentaurWarrunner = 96,
        [Display(Name = "Magnus", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/magnataur_sb.png")]
        Magnus = 97,
        [Display(Name = "Timbersaw", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/shredder_sb.png")]
        Timbersaw = 98,
        [Display(Name = "Bristleback", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/bristleback_sb.png")]
        Bristleback = 99,
        [Display(Name = "Tusk", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/tusk_sb.png")]
        Tusk = 100,
        [Display(Name = "Skywrath Mage", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/skywrath_mage_sb.png")]
        SkywrathMage = 101,
        [Display(Name = "Abaddon", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/abaddon_sb.png")]
        Abaddon = 102,
        [Display(Name = "Elder Titan", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/elder_titan_sb.png")]
        ElderTitan = 103,
        [Display(Name = "Legion Commander", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/legion_commander_sb.png")]
        LegionCommander = 104,
        [Display(Name = "Techies", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/techies_sb.png")]
        Techies = 105,
        [Display(Name = "Ember Spirit", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/ember_spirit_sb.png")]
        EmberSpirit = 106,
        [Display(Name = "Earth Spirit", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/earth_spirit_sb.png")]
        EarthSpirit = 107,
        [Display(Name = "Underlord", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/abyssal_underlord_sb.png")]
        Underlord = 108,
        [Display(Name = "Terrorblade", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/terrorblade_sb.png")]
        Terrorblade = 109,
        [Display(Name = "Phoenix", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/phoenix_sb.png")]
        Phoenix = 110,
        [Display(Name = "Oracle", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/oracle_sb.png")]
        Oracle = 111,
        [Display(Name = "Winter Wyvern", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/winter_wyvern_sb.png")]
        WinterWyvern = 112,
        [Display(Name = "Arc Warden", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/arc_warden_sb.png")]
        ArcWarden = 113,
        [Display(Name = "Monkey King", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/monkey_king_sb.png")]
        MonkeyKing = 114,
        [Display(Name = "Dark Willow", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/dark_willow_sb.png")]
        DarkWillow = 119,
        [Display(Name = "Pangolier", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/pangolier_sb.png")]
        Pangolier = 120,
        [Display(Name = "Grimstroke", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/grimstroke_sb.png")]
        Grimstroke = 121,
        [Display(Name = "Hoodwink", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/hoodwink_sb.png")]
        Hoodwink = 123,
        [Display(Name = "Void Spirit", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/void_spirit_sb.png")]
        VoidSpirit = 126,
        [Display(Name = "Snapfire", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/snapfire_sb.png")]
        Snapfire = 128,
        [Display(Name = "Mars", Description = "https://steamcdn-a.akamaihd.net/apps/dota2/images/heroes/mars_sb.png")]
        Mars = 129,

    }
}
