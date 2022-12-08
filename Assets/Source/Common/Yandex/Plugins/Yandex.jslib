mergeInto(LibraryManager.library, {

  SetActiveRateButton: function()
  {
     bridge.platform.sdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      if(value)
        unityInstance.SendMessage('CardGridGame', 'SetActiveRateButton');
      else
        unityInstance.SendMessage('CardGridGame', 'SetDeActiveRateButton');
    })
  },
});