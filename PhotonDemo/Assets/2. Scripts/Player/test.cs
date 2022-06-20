using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos.UI;


public class test : MonoBehaviour
{
    [SerializeField] private MediaPlayer mediaPlayer;
    public string url = "https://5ddfc48289114.streamlock.net/arirang/LIVE2/playlist.m3u8";

    private void Awake()
    {
        mediaPlayer.OpenMedia(new MediaPath(url, MediaPathType.AbsolutePathOrURL), autoPlay: true);

    }

  

}
