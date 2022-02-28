﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace dpp.cot
{
    public static class CotPredicates
    {
        public const string friendly = "^a-f-";
        public const string hostile = "^a-h-";
        public const string unknown = "^a-u-";
        public const string pending = "^a-p-";
        public const string assumed = "^a-a-";
        public const string neutral = "^a-n-";
        public const string suspect = "^a-s-";
        public const string joker = "^a-j-";
        public const string faker = "^a-k-";
        public const string atoms = "^a-";
        public const string air = "^a-.-A";
        public const string ground = "^a-.-G";
        public const string installation = "^a-.-G-I";
        public const string vehicle = "^a-.-G-E-V";
        public const string equipment = "^a-.-G-E";
        public const string surface = "^a-.-S";
        public const string sea = "^a-.-S";
        public const string sam = "^a-.-A-W-M-S";
        public const string subsurface = "^a-.-U";
        public const string sub = "^a-.-U";
        public const string uav = "^a-f-A-(M-F-Q|C-F-q)";
        public const string urw = "^a-f-A-M-H-Q";
        public const string bits_friend = "^b-(g|r)-f-";
        public const string bits_friendly = "^b-(g|r)-f-";
        public const string bits_hostile = "^b-(g|r)-h-";
        public const string bits_unknown = "^b-(g|r)-u-";
        public const string bits_pending = "^b-(g|r)-p-";
        public const string bits_assumed = "^b-(g|r)-a-";
        public const string bits_neutral = "^b-(g|r)-n-";
        public const string bits_suspect = "^b-(g|r)-s-";
        public const string bits_joker = "^b-(g|r)-j-";
        public const string bits_faker = "^b-(g|r)-k-";
        public const string mayday = "^a-f-.*9-1-1";
        public const string detect_nbc = "^b-d-c";
        public const string bits = "^b-";
        public const string detects = "^b-d";
        public const string radiation = "^b-d-n";
        public const string strikewarn = "^b-S";
        public const string route = "^b-m-r";
        public const string mappoint = "^b-m-p";
        public const string cuepoint = "^b-m-p-s-p-i";
        public const string click = "^b-m-p-m-c";
        public const string vpi = "^b-m-p-v-p-i";
        public const string spi = "^b-m-p-s-p-i";
        public const string point = "^b-m-p";
        public const string refpoint = "^b-m-p-r";
        public const string waypoint = "^b-m-p-w";
        public const string grid = "^b-m-g-o";
        public const string tacelint = "^b-d-r";
        public const string image = "^b-i";
        public const string kimage = "^b-i-e";
        public const string mootw = "^b-r-.-O";
        public const string alarm = "^b-l";
        public const string metoc = "^b-w";
        public const string temperature = "^b-w-A-t";
        public const string turbulence = "^b-w-A-T";
        public const string icing = "^b-w-A-I";
        public const string tstorm = "^b-w-A-S-T";
        public const string winds = "^b-w-A-W";
        public const string coverage = "^b-w-A-C";
        public const string cloudtop = "^b-w-A-C-t";
        public const string cloudbase = "^b-w-A-C-b";
        public const string cloudceiling = "^b-w-A-C-c";
        public const string cloudtotal = "^b-w-A-C-a";
        public const string not_cot = "^b-x";
        public const string any = ".*";
        public const string spare = "^$";
        public const string tasking = "^t-";
        public const string t_isr = "^t-s";
        public const string t_isr_eo = "^t-s-i-e";
        public const string t_map_topo = "^t-i-m-t";
        public const string t_cancel = "^t-z";
        public const string t_dgps = "^t-x-c-g-d";
        public const string t_strike = "^t-k";
        public const string t_destroy = "^t-k-d";
        public const string t_investigate = "^t-k-i";
        public const string t_target = "^t-k-t";
        public const string subscription = "^t-b";
        public const string t_subscription = "^t-b";
        public const string t_sub_bft = "^t-s-b";
        public const string t_lookup = "^t-x-i-l";
        public const string t_commcheck = "^t-x-a-c-c|^t-q-c-c";
        public const string t_state = "^t-x-a-s";
        public const string t_sync = "^t-x-a-s";
        public const string t_sync_sub = "^t-x-a-s-c";
        public const string t_filter = "^t-x-a-f";
        public const string t_app_open = "^t-x-a-o";
        public const string freetext = "^(t-x-f)|(b-t-f)";
        public const string sync = "^t-x-a-s";
        public const string t_medevac = "^t-x-v-m";
        public const string report = "^b-r-";
        public const string weather = "^b-w";
        public const string deprecated_graphic = "^b-g-";
        public const string deprecated_obstacle = "^b-g-.-M-O";
        public const string deprecated_area = "^b-g-.-G-G-A";
        public const string obstacle = "^a-.-G-O";
        public const string firecoord = "^b-r-F-C";
        public const string nbc = "^b-l-c-.";
        public const string nbc_chembio = "^b-l-c-b";
        public const string nbc_nuclear = "^b-l-c-n";
        public const string secmsg = "^b-x-s";
        public const string casualty = "^b-r-f-h-c";
        public const string reply = "^y-";
        public const string r_complete = "^y-c";
        public const string r_success = "^y-c-s";
        public const string r_fail = "^y-c-f";
        public const string r_failed = "^y-c-f";
        public const string r_ack = "^y-a";
        public const string r_receipt = "^y-a-r";
        public const string r_wilco = "^y-a-w";
        public const string r_canceling = "^y-s-c";
        public const string r_executing = "^y-s-e";
        public const string r_rejected = "^y-c-f-r";
        public const string r_stale = "^y-c-f-s";
        public const string r_review = "^y-s-r";
        public const string r_completion = "^y-c";
        public const string h_mensurated = "^m-i";
        public const string h_human = "^h";
        public const string h_retyped = "^h-t";
        public const string h_machine = "^m";
        public const string h_gps = "^m-g";
        public const string h_nonCoT = "-g-i-g-o";
        public const string h_gigo = "^h-g-i-g-o";
        public const string h_estimated = "^h-e";
        public const string h_calculated = "^h-c";
        public const string h_transcribed = "^h-t";
        public const string h_pasted = "^h-p";
        public const string h_magnetic = "^m-m";
        public const string h_ins = "^m-n";
        public const string h_ins_gps = "^m-g-n";
        public const string h_simulated = "^m-s";
        public const string h_configured = "^m-c";
        public const string h_radio = "^m-r";
        public const string h_passed = "^m-p";
        public const string h_fused = "^m-f";
        public const string h_tracker = "^m-a";
        public const string h_dgps = "^m-g-d";
        public const string h_eplrs = "^m-r-e";
        public const string h_plrs = "^m-r-p";
        public const string h_doppler = "^m-r-d";
        public const string h_vhf = "^m-r-v";
        public const string h_tadil = "^m-r-t";
        public const string h_tadila = "^m-r-t-a";
        public const string h_tadilb = "^m-r-t-b";
        public const string h_tadilj = "^m-r-t-j";
        public const string q_guaranteed = "^.-.-g";
        public const string q_assured = "^.-.-g";
        public const string q_deadline = "^.-.-d";
        public const string q_congestion = "^.-.-c";
        public const string q_low = "^[0-3]-.-.";
        public const string q_med = "^[4-6]-.-.";
        public const string q_high = "^[7-9]-.-.";
        public const string q_routine = "^[0-1]-.-.";
        public const string q_priority = "^[2-3]-.-.";
        public const string q_immediate = "^[4-5]-.-.";
        public const string q_flash = "^[6-7]-.-.";
        public const string q_flashover = "^[8-9]-.-.";
        public const string q_replace = "^.-r-.";
        public const string q_follow = "^.-f-.";
        public const string o_exercise = "^e-";
        public const string o_operation = "^o-";
        public const string o_simulation = "^s-";
        
        // types not in CoTtypes.xml
        public const string t_ping = "t-x-c-t";
        public const string t_pong = "t-x-c-t-r";
    }
}
