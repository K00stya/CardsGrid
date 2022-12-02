mergeInto(LibraryManager.library, {

  SetActiveRateButton: function()
  {
     ysdk.feedback.canReview()
    .then(({ value, reason }) => 
    {
      unityInstance.SendMessage('CardGridGame', 'SetActiveRateButton', value);
    })
  },
});