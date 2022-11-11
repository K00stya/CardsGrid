mergeInto(LibraryManager.library, {

  SetActiveRateButton: function()
  {
     ysdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      gameInstance.SendMessage('CardGame', 'SetActiveRateButton', value);
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
        const jSON = JSON.stringify(_data);
        gameInstance.SendMessage('CardGame', 'Load', jSON);
    })
  },

  SetToLeaderboard: function(name, value)
  {
    ysdk.getLeaderboards().then(lb =>
    { 
      lb.setLeaderboardScore(name, value);
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
          gameInstance.SendMessage('CardGame', 'RewardPlayer');
        },
        onClose: () => {
        }, 
        onError: (e) => {
          gameInstance.SendMessage('CardGame', 'NotRewardPlayer');
        }
      }
    })
  },
});