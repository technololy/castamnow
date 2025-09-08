let dotNetHelper;
let observer;

window.initializeInfiniteScroll = (instance) => {
  dotNetHelper = instance;

  // Create an Intersection Observer
  observer = new IntersectionObserver(
    (entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          dotNetHelper.invokeMethodAsync("LoadMoreData");
        }
      });
    },
    {
      root: null, // Use the viewport
      rootMargin: "0px",
      threshold: 0.1,
    }
  );

  // Observe the last item
  updateObserver();
};

window.removeInfiniteScroll = () => {
  if (observer) {
    observer.disconnect();
  }
  dotNetHelper = null;
};

window.updateObserver = () => {
  if (observer) {
    // Disconnect previous observations
    observer.disconnect();

    // Get the last cast-item
    const lastItem = document.querySelector(".cast-feed .cast-item:last-child");
    if (lastItem) {
      observer.observe(lastItem);
    }
  }
};
