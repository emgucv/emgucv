// Camera capture helpers for the Yolo demo page (called via JS interop from
// Yolo.razor). Kept as a JS module (not a global script) so it's loaded
// on-demand only when the camera option is used.

export async function startCamera(videoElement) {
    const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false });
    videoElement.srcObject = stream;
    await videoElement.play();
}

export function stopCamera(videoElement) {
    const stream = videoElement.srcObject;
    if (stream) {
        for (const track of stream.getTracks()) {
            track.stop();
        }
        videoElement.srcObject = null;
    }
}

export function captureFrame(videoElement, canvasElement) {
    const width = videoElement.videoWidth;
    const height = videoElement.videoHeight;
    if (!width || !height) {
        return Promise.reject(new Error("Camera has no frame yet -- wait for the preview to start."));
    }

    canvasElement.width = width;
    canvasElement.height = height;
    const ctx = canvasElement.getContext("2d");
    ctx.drawImage(videoElement, 0, 0, width, height);

    return new Promise((resolve, reject) => {
        canvasElement.toBlob(async (blob) => {
            if (!blob) {
                reject(new Error("Failed to encode the captured frame."));
                return;
            }
            const buffer = await blob.arrayBuffer();
            resolve(new Uint8Array(buffer));
        }, "image/png");
    });
}
