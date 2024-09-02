
# Path to your LFS storage directory
LFS_DIR="/home/jeffreybarto/Dropbox/colosseum-game-assets"

# Get list of current LFS files
git lfs ls-files -l | awk '{print $1}' > current_files.txt

# List files in the LFS storage directory
find "$LFS_DIR" -type f -exec basename {} \; > stored_files.txt

# Identify old files (those not in the current list)
comm -23 <(sort stored_files.txt) <(sort current_files.txt) > old_files.txt

# delete old files
while IFS= read -r old_file; do 
  found_file=$(find "$LFS_DIR" -type f -name "$old_file")
  if [ -n "$found_file" ]; then
    echo "Removing $found_file"
    rm "$found_file"
  else
    echo "File $file_name not found in LFS directory"
  fi
done < old_files.txt

# delete empty dirs and remove temp files
find "$LFS_DIR" -type d -empty -delete
rm current_files.txt stored_files.txt old_files.txt