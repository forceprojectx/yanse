import math
import colorsys
import svgwrite

# Constants
SVG_SIZE = 512
CENTER = SVG_SIZE / 2
NUM_CIRCLES = 12
CIRCLE_RADIUS = SVG_SIZE / 4  # 50% of icon = 128 radius
PATH_RADIUS = CENTER - CIRCLE_RADIUS  # So circle edges touch

ANGLE_PER = 360 / NUM_CIRCLES

def hsv_to_rgb_string(h, s, v):
    r, g, b = colorsys.hsv_to_rgb(h, s, v)
    return f"rgb({int(r*255)}, {int(g*255)}, {int(b*255)})"

# Create drawing
dwg = svgwrite.Drawing("circle_wedges.svg", size=(SVG_SIZE, SVG_SIZE))
clip_group = dwg.add(dwg.g())

for i in range(NUM_CIRCLES):
    angle_deg = i * ANGLE_PER
    angle_rad = math.radians(angle_deg)

    # Circle center
    cx = CENTER + PATH_RADIUS * math.cos(angle_rad)
    cy = CENTER + PATH_RADIUS * math.sin(angle_rad)

    # Unique clip path ID
    clip_id = f"clip{i}"
    clip_path = dwg.defs.add(dwg.clipPath(id=clip_id))

    # Wedge path for clipping
    start_angle = math.radians(angle_deg - ANGLE_PER / 2)
    end_angle = math.radians(angle_deg + ANGLE_PER / 2)

    x1 = CENTER + SVG_SIZE * math.cos(start_angle)
    y1 = CENTER + SVG_SIZE * math.sin(start_angle)
    x2 = CENTER + SVG_SIZE * math.cos(end_angle)
    y2 = CENTER + SVG_SIZE * math.sin(end_angle)

    wedge_path = f"M {CENTER},{CENTER} L {x1},{y1} A {SVG_SIZE},{SVG_SIZE} 0 0,1 {x2},{y2} Z"
    clip_path.add(dwg.path(d=wedge_path))

    # Circle with clip
    color = hsv_to_rgb_string(i / NUM_CIRCLES, 1.0, 1.0)
    clip_group.add(
        dwg.circle(center=(cx, cy), r=CIRCLE_RADIUS, fill=color, clip_path=f"url(#{clip_id})")
    )

# Save SVG
dwg.save()
print("✅ SVG icon generated: circle_wedges.svg")
