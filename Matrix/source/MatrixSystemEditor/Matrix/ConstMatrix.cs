using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.Matrix
 *文件名：  ConstMatrix
 *版本号：  V1.0.0.0
 *唯一标识：ed308eb8-8f73-4755-b038-3c85531900ea
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/9/2016 7:16:12 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/9/2016 7:16:12 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/

namespace MatrixSystemEditor.Matrix
{



    public class CFinal
    {

        public const double DefaultQfactor = 0.05;
        public const byte BI = 0;
        public const byte BII = 1;

        public const int EQEntireFlat = 10;
        public const int EQEntireBypass = 11;

        public const int Max_DuckerParms = 6;//bypass 
        public const int ChanelMax = 12;
        public const int MAX_SERIALNO = 5;
        public const int NormalEQMax = 10;
        public const int Max_matrixChNum = 20;


        public const int Inital_Gain_index = 36;
        public const int Inital_QV_index = 35;
        public const int Initial_FBCQV_index = 68;

        public const int Initial_ChGainIndex = 120; //0..140 --->sendas :1..141 e.g.

        public static int[] inital_freq_ary ={
         19,71,101,140,170,230,240,270,2,300            
        };
        public static int[] initial_fbcfreqAry ={
           2,19,71,101,140,83,230,240,270,280,
           110,130,150,160,170,50,190,210,26,260,
           35,42,290,300         
        };
        public static byte[] initial_fbcGainAry ={
           2,12,50,40,7,6,50,10,12,70,
           0,15,58,35,50,25,26,18,17,20,
           10,8,71,18         
        };


        //------------------------------------------------------------------------------
        public static string[] strHL_FILTER ={

"Bypass",  //0
"BW6",  //1
"BES6",  //2
"BW12",  //3
"BES12",    //4
"LK12",        //5
"BW18",        //6
"BES18", //7
"BW24",  //8
"BES24", //9
"LK24",  //10
"BW30",//11
"BES30", //12
"BW36", //13
"BES36",//14
"LK36",//15
"BW42", //16
"BES42",//17
"BW48", //18
"BES48",//18
"LK48", //20
};




        public static string[] strCompGainTable =								
 {								
 "0.0dB",  //00								
 "+1dB ",  //01								
 "+2dB ",  //02								
 "+3dB ",  //03								
 "+4dB ",  //04								
 "+5dB ",  //05								
 "+6dB ",  //06								
 "+7dB ",  //07								
 "+8dB ",  //08								
 "+9dB ",  //09								
 "+10dB",  //10								
 "+11dB",  //11								
 "+12dB",  //12								
 "+13dB",  //13								
 "+14dB",  //14								
 "+15dB",  //15								
 "+16dB",  //16								
 "+17dB",  //17								
 "+18dB",  //18								
 "+19dB",  //19								
 "+20dB",  //20								
 "+21dB",  //21								
 "+22dB",  //22								
 "+23dB",  //23								
 "+24dB",  //24								
 };

        public static string[] strChFGainTable =			
      {			
   "OFF",    //0
  "-70dB",    //1
  "-68dB",    //2
  "-65dB",    //3
  "-60dB",    //4
  "-55dB",    //5
  "-50dB",    //6

  "-48dB",    //7

  "-47dB",    //8

  "-46dB",    //9

  "-45.5dB",  //10

  "-44.5dB",  //11

  "-43.5dB",  //12

  "-42.5dB",  //13

  "-40dB",    //14

  "-38dB",    //15

  "-37.5dB",  //16

  "-37dB",    //17

  "-36.5dB",  //18

  "-35.5dB",  //19

  "-34dB",    //20

  "-33.5dB",  //21

  "-30.5dB",  //22

  "-30dB",    //23

  "-28.5dB",  //24

  "-27.5dB",  //25

  "-26.5dB",  //26

  "-25.5dB",  //27

  "-24.5dB",  //28

  "-23.5dB",  //29

  "-20dB",    //30

  "-19.5dB",  //31

  "-18.5dB",  //32

  "-17.5dB",  //33

  "-16.5dB",  //34

  "-15.5dB",  //35

  "-14.5dB",  //36

  "-13.5dB",  //37

  "-12.5dB",  //38

  "-10dB",    //39

  "-9.5dB",   //40

  "-8.5dB",   //41

  "-7.5dB",   //42

  "-7dB",     //43

  "-6.5dB",   //44

  "-6dB",     //45

  "-5.5dB",   //46

  "-5dB",     //47

  "-4.5dB",   //48

  "-4dB",     //49

  "-3.5dB",   //50

  "-2.5dB",   //51

  "-1.5dB",   //52

  "0dB",     //53  

  "0.5dB",    //54

  "1dB",      //55

  "1.5dB",    //56

  "2dB",      //57

  "2.5dB",    //58

  "3dB",      //59

  "3.5dB",    //60

  "4dB",      //61

  "4.5dB",    //62

  "5dB",      //63

  "5dB",      //64

  "5.5dB",     //65

  "5.5dB",     //66

  "6.0dB",     //67

  "6.0dB",     //68

 "6.5dB",     //69

  "7.0dB",     //70

 "7.0dB",     //71

 "7.5dB",     //72

  "7.5dB",     //73

 "8.0dB",     //74

  "8.0dB",     //75

  "8.5dB",     //76

  "9dB",       //77

  "9.5dB",     //78

  "9.8dB",     //79

  "10dB"       //80
		
};

        #region ducker array define below
        public static string[] strDuck_threshold =
{

    "-60dB",	//00
	"-58.8dB",	//01
	"-57.5dB",	//02
	"-56.3dB",	//03
	"-55dB",	//04
	"-53.8dB",	//05
	"-52.5dB",	//06
	"-51.3dB",	//07
	"-50dB",	//08
	"-48.8dB",	//09
	"-47.5dB",	//10
	"-46.3dB",	//11
	"-45dB",	//12
	"-43.8dB",	//13
	"-42.5dB",	//14
	"-41.3dB",	//15
	"-40dB",	//16
	"-38.8dB",	//17
	"-37.5dB",	//18
	"-36.3dB",	//19
	"-35dB",	//20
	"-33.8dB",	//21
	"-32.5dB",	//22
	"-31.3dB",	//23
	"-30dB",	//24
	"-28.8dB",	//25
	"-27.5dB",	//26
	"-26.3dB",	//27
	"-25dB",	//28
	"-23.8dB",	//29
	"-22.5dB",	//30
	"-21.3dB",	//31
	"-20dB",	//32
	"-18.8dB",	//33
	"-17.5dB",	//34
	"-16.3dB",	//35
	"-15dB",	//36
	"-13.8dB",	//37
	"-12.5dB",	//38
	"-11.3dB",	//39
	"-10dB",	//40
	"-8.8dB",	//41
	"-7.5dB",	//42
	"-6.3dB",	//43
	"-5dB",	//44
	"-3.8dB",	//45
	"-2.5dB",	//46
	"-1.3dB",	//47
	"0dB"	//48
};
        public static string[] strDuck_Depth =
 {
     "0dB",	//00
	"2dB",	//01
	"4dB",	//02
	"6dB",	//03
	"8dB",	//04
	"10dB",	//05
	"12dB",	//06
	"14dB",	//07
	"16dB",	//08
	"18dB",	//09
	"20dB",	//10
	"22dB",	//11
	"24dB",	//12
	"26dB",	//13
	"28dB",	//14
	"30dB",	//15
	"32dB",	//16
	"34dB",	//17
	"36dB",	//18
	"38dB",	//19
	"40dB",	//20
	"42dB",	//21
	"44dB",	//22
	"46dB",	//23
	"48dB",	//24
	"50dB",	//25
	"52dB",	//26
	"54dB",	//27
	"56dB",	//28
	"58dB",	//29
	"60dB",	//30
	"62dB",	//31
	"64dB",	//32
	"66dB",	//33
	"68dB",	//34
	"70dB",	//35
	"72dB",	//36
	"74dB",	//37
	"76dB",	//38
	"78dB",	//39
	"80dB",	//40
	"82dB",	//41
	"84dB",	//42
	"86dB",	//43
	"88dB",	//44
	"90dB",	//45
	"92dB",	//46
	"94dB",	//47
	"96dB",	//48
 };

        public static string[] strDuck_Attack =
  {
     "10mS",	//00
	"20mS",	//01
	"30mS",	//02
	"41mS",	//03
	"51mS",	//04
	"61mS",	//05
	"71mS",	//06
	"81mS",	//07
	"92mS",	//08
	"102mS",	//09
	"112mS",	//10
	"122mS",	//11
	"132mS",	//12
	"143mS",	//13
	"153mS",	//14
	"163mS",	//15
	"173mS",	//16
	"183mS",	//17
	"194mS",	//18
	"204mS",	//19
	"214mS",	//20
	"224mS",	//21
	"234mS",	//22
	"245mS",	//23
	"255mS",	//24
	"265mS",	//25
	"275mS",	//26
	"285mS",	//27
	"296mS",	//28
	"306mS",	//29
	"316mS",	//30
	"326mS",	//31
	"336mS",	//32
	"347mS",	//33
	"357mS",	//34
	"367mS",	//35
	"377mS",	//36
	"387mS",	//37
	"398mS",	//38
	"408mS",	//39
	"418mS",	//40
	"428mS",	//41
	"438mS",	//42
	"449mS",	//43
	"459mS",	//44
	"469mS",	//45
	"479mS",	//46
	"489mS",	//47
	"500mS",	//48  
  };

        public static string[] strDuck_Hold =
 {

   "10mS",	//00
	"218mS",	//01
	"426mS",	//02
	"634mS",	//03
	"842mS",	//04
	"1051mS",	//05
	"1259mS",	//06
	"1467mS",	//07
	"1675mS",	//08
	"1883mS",	//09
	"2091mS",	//10
	"2299mS",	//11
	"2507mS",	//12
	"2715mS",	//13
	"2923mS",	//14
	"3132mS",	//15
	"3340mS",	//16
	"3548mS",	//17
	"3756mS",	//18
	"3964mS",	//19
	"4172mS",	//20
	"4380mS",	//21
	"4588mS",	//22
	"4796mS",	//23
	"5004mS",	//24
	"5213mS",	//25
	"5421mS",	//26
	"5629mS",	//27
	"5837mS",	//28
	"6045mS",	//29
	"6253mS",	//30
	"6461mS",	//31
	"6669mS",	//32
	"6877mS",	//33
	"7085mS",	//34
	"7294mS",	//35
	"7502mS",	//36
	"7710mS",	//37
	"7918mS",	//38
	"8126mS",	//39
	"8334mS",	//40
	"8542mS",	//41
	"8750mS",	//42
	"8958mS",	//43
	"9166mS",	//44
	"9375mS",	//45
	"9583mS",	//46
	"9791mS",	//47
	"10000mS",	//48
 };
        //------------------------------------------------------------------------------
        public static string[] strDuck_Release =
 {
	"10mS",	//00
	"1259mS",	    //01
	"2508mS",	    //02
	"3757mS",	    //03
	"5006mS",	    //04
	"6255mS",	    //05
	"7504mS",	    //06
	"8753mS",	    //07
	"10002mS",	//08
	"11251mS",	//09
	"12500mS",	//10
	"13749mS",	//11
	"14998mS",	//12
	"16247mS",	//13
	"17496mS",	//14
	"18745mS",	//15
	"19994mS",	//16
	"21243mS",	//17
	"22492mS",	//18
	"23741mS",	//19
	"24990mS",	//20
	"26239mS",	//21
	"27488mS",	//22
	"28737mS",	//23
	"29986mS",	//24
	"31235mS",	//25
	"32484mS",	//26
	"33733mS",	//27
	"34982mS",	//28
	"36231mS",	//29
	"37480mS",	//30
	"38729mS",	//31
	"39978mS",	//32
	"41227mS",	//33
	"42476mS",	//34
	"43725mS",	//35
	"44974mS",	//36
	"46223mS",	//37
	"47472mS",	//38
	"48721mS",	//39
	"49970mS",	//40
	"51219mS",	//41
	"52468mS",	//42
	"53717mS",	//43
	"54966mS",	//44
	"56215mS",	//45
	"57464mS",	//46
	"58713mS",	//47
	"60000mS",	//48

 };

        #endregion


    };



    public class MatrixCMD
    {

        public const int F_InputPhaseGain = 65;
        public const int F_OutputPhaseGain = 66;
        public const int F_InputExpGATE = 67;
        public const int F_OutputExpGATE = 68;
        public const int F_InputEQ = 69;
        public const int F_OutputEQ = 70;
        public const int F_InputCOMP = 71;
        public const int F_OutputCOMP = 72;
        public const int F_InputDelay = 73;
        public const int F_OutputDelay = 74;

        public const int F_DuckerParameter = 75;
        public const int F_DuckerInputMixer = 76;
        public const int F_RDPortOutSourceSet = 77;
        public const int F_MatrixMixer = 78;
        public const int F_PagingMixer = 79;
        public const int F_ReadStatus = 80;
        public const int F_InputDG411Gain = 81;
        public const int F_Ack = 82;
        public const int F_InputEQFlat = 83;
        public const int F_OutEQFlat = 84;
        public const int F_Copy = 85;
        public const int F_InputDC48VFlag = 86;
        //Reserve 87
        public const int F_WrDevInfo = 88;
        public const int F_RdDevInfo = 89;
        //Reserve 90
        public const int F_Store = 91;
        public const int F_Recall = 92;
        //Matrix cmd define below:
        public const int F_LoadFromPC = 93;
        public const int F_RDDefaultSetting = 94;
        public const int F_ReturnDefaultSetting = 95;

        public const int F_RecallCurrentScene = 96;
        public const int F_GetPresetList = 97;

        public const int F_Delete = 98;
        public const int F_SigMeters = 99;
        //Reserve 100
        public const int F_Mute = 101;

        public const int F_MemoryExport = 102;
        public const int F_MemoryInport = 105;
        public const int F_MemoryImportAck = 106;

        public const int F_WrDevSerialNO = 107;
        public const int F_SetPageZoneIndex = 108;
        public const int F_RelayControl = 109;

        public const int F_FBCSetting = 110;
        public const int F_GetFBCStatus = 111;
        public const int F_FBCGainSetting = 112;
        public const int F_FBCFilterStatus = 113;
        public const int F_FBCFilterGain_F08 = 114;
        public const int F_FBCFilterGain_F16 = 115;
        public const int F_FBCFilterGain_F24 = 116;
        public const int F_FBCFilterFreq_F08 = 117;
        public const int F_FBCFilterFreq_F16 = 118;
        public const int F_FBCFilterFreq_F24 = 119;

        public const int F_InpuGain = 120;
        public const int F_GetInpuGain = 121;

        public const int F_InpuPhase = 122;
        public const int F_GetInpuPhase = 123;

        public const int F_OutputGain = 124;
        public const int F_GetOutputGain = 125;

        public const int F_OutputPhase = 126;
        public const int F_GetOutputPhase = 127;

        public const int F_GetInpuDelay = 128;
        public const int F_GetOutputDelay = 129;

        public const int F_GetMatrixMixer = 130;

        public const int F_GetInMeter = 131;
        public const int F_GetOutMeter = 132;
        public const int F_BGMSelect = 133;

        //PC Communication ,MCU IAP Command
        public const int IAP_MODE_EXIT = 0XE0;
        public const int IAP_PROGRAMING = 0xF0;
        public const int IAP_FINISH = 0xF1;

        public const int IAP_ENTER_IN_APP = 0xEE;		//Enter IAP Mode in application
        public const int IAP_MODE_ENTER_READY = 0xEF;    //IAP Mode Prepare ready, 

        //PC Communication ,DSP Firmware Update Command
        public const int F_DSPFirmwareUpdate = 0xf2;     //DSP Firmware update 
        public const int F_UpdateProgress = 0xf3;
        public const int F_ReadDSPCode = 0xf4;
        public const int F_SendMCUStart = 0xf5;

    }
}
