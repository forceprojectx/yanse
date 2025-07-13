import os
import shutil

# List of target Chinese characters
characters = [
    "红", "橙", "黄", "绿", "蓝", "紫", "粉", "棕", "灰", "黑", "白", "金", "银", "青", "栗", "米", "靛", "品", "浅", "蓝", "藏", "天", "青", "珊", "橄"
]

# Convert characters to codepoint strings
codepoints = [str(ord(c)) for c in characters]

# Directories (update these paths)
src_dir = "C:/Users/force/Source/repos/makemeahanzi/svgs-still"
dst_dir = "C:/Users/force/Source/repos/yanse/yanse/Resources/Images"

# Ensure destination exists
os.makedirs(dst_dir, exist_ok=True)

# Copy matching files
for filename in os.listdir(src_dir):
    for code in codepoints:
        if filename.startswith(code):
            src_path = os.path.join(src_dir, filename)
            new_name = "hanzi_" + filename.replace('-', '_')  # Add prefix
            dst_path = os.path.join(dst_dir, new_name)
            shutil.copy2(src_path, dst_path)
            print(f"Copied: {filename} to {dst_path}")
            break  # Skip further checks once matched
