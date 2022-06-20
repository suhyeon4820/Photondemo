using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DBManager 
{
    [Serializable] // 구조체 인스펙터에 노출
    public class AllPlayworldData
    {
        public int pageCount;
        public int totalCount;
        public PlyaworldData[] data;
        public int res_code;
        public string res_msg;
    }

    [Serializable]
    public class PlyaworldData
    {
        public string id;
        public string title;
        public string eventTimes;
        public string stroeName;
        public string thumbnailUrl;
    }

    // 플레이어 ID --------------------------------
    [Serializable]
    public class PlayerIdData
    {
        public int pageCount;
        public int totalCount;
        public PlayerIdDetailData data;
        public int res_code;
        public string res_msg;
    }
    [Serializable]
    public class PlayerIdDetailData
    {
        public string title;
        public string introduction;
        public string tags;
        public string link;
        public string eventTime;
        public PlayserSpaceData[] spaces;
    }
    [Serializable]
    public class PlayserSpaceData
    {
        public int id;
        public string title;
        public string resourcePath;
    }
    // 유튜브 주소 --------------------------------
    [Serializable]
    public class SpaceData
    {
        public int pageCount;
        public int totalCount;
        public SpaceDetailData data;
        public int res_code;
        public string res_msg;
    }
    [Serializable]
    public class SpaceDetailData
    {
        public string title;
        public string spacesType;
        public string resourcePath;
        public SpaceContent[] spaceContent;
    }
    [Serializable]
    public class SpaceContent
    {
        public string title;
        public string type;
        public string url;
    }
}
