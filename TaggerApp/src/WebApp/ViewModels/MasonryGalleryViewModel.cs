using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebApp.Services;

namespace WebApp.ViewModels {
    public class MasonryGalleryViewModel : ComponentBase {
        public const int pageSize = 10;
        protected string result;
        private DotNetObjectReference<MasonryGalleryViewModel> _objRef;
        protected bool IsScrollTrackingEnabled { get; set; } = false;
        protected List<string> Images { get; set; }

        [Inject]
        protected IJSRuntime JS { get; set; }

        [Inject]
        protected DataService DataService { get; set; }

        public void Dispose() {
            _objRef?.Dispose();
        }

        [JSInvokable]
        public bool IsAtWindowBottom(double contentScrollTop, double contentHeight, double containerHeight) {
            bool retVal = (contentScrollTop + contentHeight) >= containerHeight;
            return retVal;
        }

        [JSInvokable]
        public bool IsNearWindowBottom(double contentScrollTop, double contentHeight, double containerHeight) {
            bool retVal = (contentScrollTop + contentHeight) > (containerHeight - 100);
            return retVal;
        }

        [JSInvokable]
        public async Task LoadMoreImages(int pageIndex) {
            //await JS.InvokeVoidAsync("eval", "console.log('LoadMoreImages')");
            var newImages = await DataService.GetFiles(pageIndex, pageSize);
            Images.AddRange(newImages);
            StateHasChanged();
            await JS.InvokeVoidAsync("eval", $"console.log('LoadMoreImages | totalImages:{Images.Count} newImages:{newImages.Count}')");
            if (newImages != null && newImages.Any())
                _ = await JS.InvokeAsync<string>("blazorExtensions.triggerMasonry", newImages.Count);
            //TODO: Glenn - This list of new images should be added to the Blazor list with Images.AddRange(newImages)
            //TODO: Glenn - But this will have the bad positioning effect, we need to trigger the imagesloaded function
            //TODO: Glenn - Also this should be doable according to https://stackoverflow.com/questions/64593058/blazor-force-full-render-instead-of-differential-render
            //TODO: Glenn - But I was not able to get it working correctly, hence the roundtrip and pushing the string to JavaScript
            /*if (newImages != null && newImages.Any())
                _ = await JS.InvokeAsync<string>("blazorExtensions.triggerMasonry", string.Join("#", newImages));*/
        }

        public async Task ToggleDotNetTrackScroll() {
            _ = await JS.InvokeAsync<string>("blazorExtensions.toggleTrackScroll", _objRef);
        }

        [JSInvokable]
        public bool ToggleTrackScroll() { IsScrollTrackingEnabled = !IsScrollTrackingEnabled; return IsScrollTrackingEnabled; }

        protected override async Task OnInitializedAsync() {
            var files = await DataService.GetFiles(0, pageSize);
            Images = files;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                _objRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("blazorExtensions.toggleTrackScroll", _objRef);
                StateHasChanged();

                await JS.InvokeVoidAsync("blazorExtensions.initMasonry", _objRef);
            }
        }

        public bool IsImage(string filePath) {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".avif" }; // Add other image formats as needed
            return extensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsVideo(string filePath) {
            var extensions = new[] { ".mp4", ".webm", ".ogg" }; // Add other video formats as needed
            return extensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}
