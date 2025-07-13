from PIL import Image, ImageDraw
import math
import colorsys
import os

# Base image size (design resolution)
BASE_SIZE = 512
NUM_CIRCLES = 12
CIRCLE_DIAMETER = BASE_SIZE // 2
CIRCLE_RADIUS = CIRCLE_DIAMETER // 2
CENTER = BASE_SIZE // 2
PATH_RADIUS = (BASE_SIZE - CIRCLE_DIAMETER) // 2
ANGLE_PER = 360 / NUM_CIRCLES

# Android icon sizes
android_icons = {
    "mipmap-mdpi": 48,
    "mipmap-hdpi": 72,
    "mipmap-xhdpi": 96,
    "mipmap-xxhdpi": 144,
    "mipmap-xxxhdpi": 192
}

# Output base path
OUTPUT_DIR = "android_icons"

def hsv_to_rgb255(h, s, v):
    r, g, b = colorsys.hsv_to_rgb(h, s, v)
    return int(r * 255), int(g * 255), int(b * 255)

# Generate the full base image (512x512)
final = Image.new("RGBA", (BASE_SIZE, BASE_SIZE), (255, 255, 255, 0))

for i in range(NUM_CIRCLES):
    angle_deg = i * ANGLE_PER
    angle_rad = math.radians(angle_deg)

    cx = CENTER + PATH_RADIUS * math.cos(angle_rad)
    cy = CENTER + PATH_RADIUS * math.sin(angle_rad)

    circle_img = Image.new("RGBA", (BASE_SIZE, BASE_SIZE), (0, 0, 0, 0))
    draw_circle = ImageDraw.Draw(circle_img)

    color = hsv_to_rgb255(i / NUM_CIRCLES, 1.0, 1.0)
    draw_circle.ellipse([
        cx - CIRCLE_RADIUS, cy - CIRCLE_RADIUS,
        cx + CIRCLE_RADIUS, cy + CIRCLE_RADIUS
    ], fill=color)

    # Mask for wedge
    mask = Image.new("L", (BASE_SIZE, BASE_SIZE), 0)
    draw_mask = ImageDraw.Draw(mask)
    draw_mask.pieslice(
        [0, 0, BASE_SIZE, BASE_SIZE],
        start=angle_deg - ANGLE_PER / 2,
        end=angle_deg + ANGLE_PER / 2,
        fill=255
    )

    final.paste(circle_img, (0, 0), mask)

# Create and save icons in required sizes
for folder, size in android_icons.items():
    icon = final.resize((size, size), Image.LANCZOS)
    folder_path = os.path.join(OUTPUT_DIR, folder)
    os.makedirs(folder_path, exist_ok=True)
    icon_path = os.path.join(folder_path, "ic_launcher.png")
    icon.save(icon_path)

print("✅ Android icons generated successfully!")
