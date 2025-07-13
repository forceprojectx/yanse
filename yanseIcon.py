from PIL import Image, ImageDraw
import math
import colorsys

# Settings
ICON_SIZE = 256
NUM_CIRCLES = 12
SMALL_DIAMETER = ICON_SIZE // 2
SMALL_RADIUS = SMALL_DIAMETER // 2
CENTER = ICON_SIZE // 2
PATH_RADIUS = (ICON_SIZE - SMALL_DIAMETER) // 2  # Distance from center to circle center

def hsv_to_rgb255(h, s, v):
    r, g, b = colorsys.hsv_to_rgb(h, s, v)
    return int(r * 255), int(g * 255), int(b * 255)

# Create the image
img = Image.new("RGBA", (ICON_SIZE, ICON_SIZE), (255, 255, 255, 0))
draw = ImageDraw.Draw(img)

for i in reversed(range(NUM_CIRCLES)):
    angle_deg = i * (360 / NUM_CIRCLES)
    angle_rad = math.radians(angle_deg)

    # Position of each circle center
    x = CENTER + PATH_RADIUS * math.cos(angle_rad)
    y = CENTER + PATH_RADIUS * math.sin(angle_rad)

    # Circle bounding box
    bbox = [
        x - SMALL_RADIUS,
        y - SMALL_RADIUS,
        x + SMALL_RADIUS,
        y + SMALL_RADIUS
    ]

    # RGB color based on HSV hue wheel
    hue = i / NUM_CIRCLES  # 0 to 1
    color = hsv_to_rgb255(hue, 1.0, 1.0)

    # Draw circle
    draw.ellipse(bbox, fill=color)

# Save
img.save("yanse.png")
