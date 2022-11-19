mergeInto(LibraryManager.library, {

  SetActiveRateButton: function()
  {
     ysdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      myGameInstance.SendMessage('CardGridGame', 'SetActiveRateButton', value);
    })
  },

  RateGame: function ()
  {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      if (value) 
      {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) =>
        {
          console.log(feedbackSent);
        })
      } 
      else {
        console.log(reason)
      }
    }) 
  },


    SaveOnYandex: function(data)
    {
      try {
        var dataString = UTF8ToString(data);
        var myObj = JSON.parse(dataString);
        player.setData(myObj, false).then(() => {
        console.log('Cloud saves are installed');
        });
        } catch (e) {
          console.error('CRASH Save Cloud: ', e.message);
      }
    },

    LoadFromYandex: function()
    {
        try {
            player.getData().then(data => {
                if (data) {
                    myGameInstance.SendMessage('CardGridGame', 'Load', JSON.stringify(data));
                } else {
                    ResetProgress();
                }
            }).catch(() => {console.error('getData Error!');});
        } catch (e) {
            console.error('CRASH Load Saves Cloud: ', e.message);
            setTimeout(function () {
                LoadFromYandex();
            }, 1000);
        }
    },

  SetToLeaderboard: function(name, value)
  {
    ysdk.getLeaderboards().then(lb =>
    { 
      lb.setLeaderboardScore(name, value);
    });
  },

  GetLeaderBoard: function(name)
  {
    ysdk.getLeaderboards().then(lb => 
    {
      lb.getLeaderboardEntries(name, { quantityTop: 10, includeUser: true, quantityAround: 3 })
      .then(res => console.log(res));
    });
  },

  GetLang: function()
  {
    var lang = ysdk.environment.i18n.lang;
    var bufSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufSize);
    stringToUTF8(lang, buffer, bufSize);
    return buffer;
  },

  ShowFullScreenAd: function()
  {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          // some action after close
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  ShowRewardAd: function()
  {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
        },
        onRewarded: () => {
          myGameInstance.SendMessage('CardGridGame', 'Rewarded');
        },
        onClose: () => {
          myGameInstance.SendMessage('CardGridGame', 'AdRewardClose');
        }, 
        onError: (e) => {
          myGameInstance.SendMessage('CardGridGame', 'NotRewardPlayer');
        }
      }
    })
  },

  NewSave: function()
  {
    console.log("NewSave");
  },

  ReSave: function()
  {
    console.log("ReSave");
  },

  LoadSave: function()
  {
    console.log("LoadSave");
  },

});