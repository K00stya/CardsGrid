mergeInto(LibraryManager.library, {

  SetActiveRateButton: function()
  {
     ysdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      myGameInstance.SendMessage('CardGame', 'SetActiveRateButton', value);
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
    var dataString = UTF8ToString(data);
    var obj = JSON.parse(dataString);
    player.setData(obj);
  },

  LoadFromYandex: function()
  {
    player.getData().then(_data => 
    {
      try {
        player.getData(["saves"]).then(data => {
          if (data.saves) {
            myGameInstance.SendMessage('CardGame', 'Load', JSON.stringify(data.saves));
          } else {
            myGameInstance.SendMessage('CardGame', 'Load', "");
          }
        }).catch(() => {
          console.error('getData Error!');
        });
      } catch (e) {
        console.error('CRASH Load Saves Cloud: ', e.message);
        setTimeout(function () {
          LoadFromYandex();
        }, 1000);
      }
    })
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
          myGameInstance.SendMessage('CardGame', 'Rewarded');
        },
        onClose: () => {
          myGameInstance.SendMessage('CardGame', 'AdRewardClose');
        }, 
        onError: (e) => {
          myGameInstance.SendMessage('CardGame', 'NotRewardPlayer');
        }
      }
    })
  },
});