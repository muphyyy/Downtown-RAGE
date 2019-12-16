using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowntownRP.Game.Animations
{
    public class Animations : Script
    {
        // anim names taken from https://alexguirre.github.io/animations-list/
        List<PlayerAnim> PlayerAnims = new List<PlayerAnim>
        {
            new PlayerAnim("~r~Parar anim",               "",                                                             ""),
            new PlayerAnim("¿Donde estoy?",               "amb@world_human_guard_patrol@male@idle_a",                     "idle_c"),
            new PlayerAnim("¿Que es eso?",                "amb@world_human_guard_patrol@male@idle_b",                     "idle_e"),
            new PlayerAnim("AplaudirF",                   "amb@world_human_cheering@female_d",                            "base"),
            new PlayerAnim("AplaudirM",                   "amb@world_human_cheering@male_d",                              "base"),
            new PlayerAnim("Aplaudir sin ganas",          "amb@world_human_cheering@male_e",                              "base"),
            new PlayerAnim("Apoyarse en la pared",        "amb@world_human_leaning@male@wall@back@foot_up@base",          "base"),
            new PlayerAnim("Apoyarse manos juntas",       "amb@world_human_leaning@male@wall@back@hands_together@base",   "base"),
            new PlayerAnim("Apuntar en papel",            "amb@medic@standing@timeofdeath@base",                          "base"),
            new PlayerAnim("Arreglar bombilla",           "amb@prop_human_movie_bulb@idle_a",                             "idle_b"),
            new PlayerAnim("Avisar por radio",            "amb@code_human_police_investigate@idle_a",                     "idle_b"),
            new PlayerAnim("Brazos cruzadosF",            "amb@world_human_hang_out_street@female_arms_crossed@base",     "base"),
            new PlayerAnim("Brazos cruzadosM",            "amb@world_human_hang_out_street@male_c@base",                  "base"),
            new PlayerAnim("Borracho 1",                  "amb@world_human_bum_standing@drunk@idle_a",                    "idle_a"),
            new PlayerAnim("Borracho 2",                  "amb@world_human_bum_standing@drunk@idle_a",                    "idle_b"),
            new PlayerAnim("Borracho 3",                  "amb@world_human_bum_standing@drunk@idle_a",                    "idle_c"),
            new PlayerAnim("Celebrar",                    "amb@world_human_cheering@male_b",                              "base"),
            new PlayerAnim("Con ritmoM",                  "amb@world_human_partying@male@partying_beer@idle_a",           "idle_b"),
            new PlayerAnim("Con ritmoF",                  "amb@world_human_partying@female@partying_beer@base",           "base"),
            new PlayerAnim("Charlar",                     "amb@world_human_hang_out_street@male_a@idle_a",                "idle_c"),
            new PlayerAnim("Decepcion",                   "amb@world_human_bum_standing@depressed@idle_a",                "idle_c"),
            new PlayerAnim("Depresion",                   "amb@world_human_bum_standing@depressed@idle_a",                "idle_a"),
            new PlayerAnim("Estirar musculos",            "amb@world_human_muscle_flex@arms_at_side@idle_a",              "idle_a"),
            new PlayerAnim("Flexion",                     "amb@world_human_sit_ups@male@idle_a",                          "idle_a"),
            new PlayerAnim("Fumar apoyado 1",             "amb@world_human_leaning@male@wall@back@smoking@idle_a",        "idle_a"),
            new PlayerAnim("Fumar apoyado 2",             "amb@world_human_leaning@male@wall@back@smoking@idle_a",        "idle_b"),
            new PlayerAnim("Fumar apoyado 3",             "amb@world_human_leaning@male@wall@back@smoking@idle_a",        "idle_c"),
            new PlayerAnim("Fumar apoyado 4",             "amb@world_human_leaning@male@wall@back@smoking@idle_a",        "idle_d"),
            new PlayerAnim("Fumar apoyado 5",             "amb@world_human_leaning@female@smoke@idle_a",                  "idle_a"),
            new PlayerAnim("Fumar tranquilo",             "amb@world_human_aa_smoke@male@idle_a",                         "idle_a"),
            new PlayerAnim("Fumar tranquila",             "amb@world_human_leaning@female@smoke@idle_a",                  "idle_a"),
            new PlayerAnim("Grabar mobil",                "amb@world_human_mobile_film_shocking@male@idle_a",             "idle_c"),
            new PlayerAnim("Levantar Mano",               "mp_am_hold_up",                                                "handsup_base"),
            new PlayerAnim("Limpiar manos agachado",      "amb@world_human_bum_wash@male@high@idle_a",                    "idle_a"),
            new PlayerAnim("Me duele la espalda",         "amb@prop_human_bum_bin@idle_b",                                "idle_d"),
            new PlayerAnim("Mirar el suelo",              "amb@medic@standing@kneel@idle_a",                              "idle_a"),
            new PlayerAnim("Nervioso",                    "amb@world_human_bum_standing@twitchy@base",                    "base"),
            new PlayerAnim("Posar",                       "amb@world_human_muscle_flex@arms_in_front@idle_a",             "idle_a"),
            new PlayerAnim("SentarseM",                   "amb@world_human_picnic@male@idle_a",                           "idle_a"),
            new PlayerAnim("SentarseF",                   "amb@world_human_picnic@female@idle_a",                         "idle_a"),
            new PlayerAnim("Sentarse pies cruzados",      "amb@prop_human_seat_chair@female@arms_folded@base",            "base"),
            new PlayerAnim("Sentarse piernas cruzadas",   "amb@prop_human_seat_chair@female@legs_crossed@base",           "base"),
            new PlayerAnim("Sentarse con pie fuera",      "amb@prop_human_seat_chair@male@right_foot_out@base",           "base"),
            new PlayerAnim("Sentarse tímido",             "amb@prop_human_seat_chair@female@proper@base",                 "base"),
            new PlayerAnim("Sentarse cansado 1",          "amb@prop_human_seat_chair@male@left_elbow_on_knee@base",       "base"),
            new PlayerAnim("Sentarse cansado 2",          "amb@prop_human_seat_chair@male@elbows_on_knees@base",          "base"),
            new PlayerAnim("Sentarse apoyado",            "amb@prop_human_seat_chair@male@recline_b@base_b",              "base_b"),
            new PlayerAnim("Sentarse con musica",         "amb@prop_human_seat_strip_watch@bit_thuggy@base",              "base"),
            new PlayerAnim("Sentarse a disfrutar 1",      "amb@prop_human_seat_strip_watch@bit_thuggy@idle_a",            "idle_c"),
            new PlayerAnim("Sentarse a disfrutar 2",      "amb@prop_human_seat_strip_watch@bouncy_guy@idle_a",            "idle_a"),
            new PlayerAnim("Sentarse a disfrutar 3",      "amb@prop_human_seat_strip_watch@bouncy_guy@idle_a",            "idle_b"),
            new PlayerAnim("Sentarse a disfrutar 4",      "amb@prop_human_seat_strip_watch@bouncy_guy@idle_a",            "idle_c"),
            new PlayerAnim("Sentarse piernas abiertas",   "amb@prop_human_seat_chair@male@generic@base",                  "base"),
            new PlayerAnim("Timida",                      "amb@world_human_hang_out_street@female_hold_arm@base",         "base"),
            new PlayerAnim("Tranquilo tranquilo",         "amb@code_human_police_crowd_control@idle_a",                   "idle_c"),
            new PlayerAnim("Vigilar el area",             "amb@world_human_guard_patrol@male@idle_a",                     "idle_a")
        };

        [RemoteEvent("RequestAnims")]
        public void RequestAnims(Client player, object[] arguments)
        {
            player.TriggerEvent("ReceiveAnims", NAPI.Util.ToJson(PlayerAnims.Select(m => m.Name)));
        }

        [RemoteEvent("PlayAnim")]
        public void PlayAnim(Client player, object[] arguments)
        {
            if (arguments.Length < 1) return;

            int id = Convert.ToInt32(arguments[0]);
            if (id < 0 || id >= PlayerAnims.Count) return;

            if (id == 0)
            {
                player.StopAnimation();
                player.TriggerEvent("SetAnimPlaying", false);
            }
            else
            {
                player.PlayAnimation(PlayerAnims[id].AnimDict, PlayerAnims[id].AnimName, (int)(AnimationFlags.Loop));
                player.TriggerEvent("SetAnimPlaying", true);
            }
        }

        [RemoteEvent("StopPlayerAnim")]
        public void StopPlayerAnim(Client player, object[] arguments)
        {
            player.StopAnimation();
            player.TriggerEvent("SetAnimPlaying", false);
        }
    }
}
