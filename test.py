#!/usr/bin/env python3
"""
Script to analyze and display the structure of a .NET project on macOS
while ignoring unnecessary folders and files.
"""

import os
import sys
import argparse
from datetime import datetime

# Directories to ignore
IGNORE_DIRS = {
    "bin",
    "obj",
    "node_modules",
    ".vs",
    ".git",
    "packages",
    "TestResults",
    ".github",
    ".vscode"
}

# File extensions to ignore
IGNORE_EXTENSIONS = {
    ".user",
    ".suo",
    ".tmp",
    ".cache",
    ".log",
    ".vspscc",
    ".vssscc"
}

# Files to ignore
IGNORE_FILES = {
    "package-lock.json",
    "yarn.lock",
    ".DS_Store",
    "Thumbs.db"
}

# File type icons
FILE_ICONS = {
    ".cs": "🔷",     # C# file
    ".csproj": "🔶", # Project file
    ".sln": "🔸",    # Solution file
    ".json": "📋",   # JSON file
    ".xml": "📰",    # XML file
    ".md": "📝",     # Markdown
    ".yml": "⚙️",    # YAML
    ".yaml": "⚙️",   # YAML
    ".config": "⚙️", # Config file
    ".dll": "📦",    # DLL
    ".exe": "⚡",    # Executable
    # Default will be "📄"
}


def get_project_structure(path, current_depth=0, indent="", max_depth=10, include_files=True):
    """
    Recursively analyze and print the project structure
    """
    if current_depth > max_depth:
        return

    try:
        # Get all items in the directory
        items = os.listdir(path)
        
        # Sort items: directories first, then files, both alphabetically
        dir_items = []
        file_items = []
        
        for item in items:
            item_path = os.path.join(path, item)
            if os.path.isdir(item_path):
                dir_items.append(item)
            else:
                file_items.append(item)
                
        dir_items.sort()
        file_items.sort()
        items = dir_items + file_items
        
        for item in items:
            item_path = os.path.join(path, item)
            is_dir = os.path.isdir(item_path)

            # Skip if it's a directory that should be ignored
            if is_dir and item in IGNORE_DIRS:
                continue

            # Skip if it's a file that should be ignored
            if not is_dir:
                if item in IGNORE_FILES:
                    continue
                
                _, ext = os.path.splitext(item)
                if ext in IGNORE_EXTENSIONS:
                    continue

            # Print the current item
            if is_dir:
                print(f"{indent}📁 {item}")
                get_project_structure(
                    item_path, 
                    current_depth + 1, 
                    indent + "  ", 
                    max_depth, 
                    include_files
                )
            elif include_files:
                # Determine file icon based on extension
                _, ext = os.path.splitext(item)
                file_icon = FILE_ICONS.get(ext.lower(), "📄")
                print(f"{indent}{file_icon} {item}")
                
    except PermissionError:
        print(f"{indent}🔒 Permission denied: {path}")
    except Exception as e:
        print(f"{indent}❌ Error accessing {path}: {e}")


def main():
    parser = argparse.ArgumentParser(description="Analyze .NET project structure.")
    parser.add_argument("project_path", help="Path to the .NET project")
    parser.add_argument("-f", "--files", action="store_true", default=True, 
                      help="Include files in the output (default: True)")
    parser.add_argument("-d", "--depth", type=int, default=10, 
                      help="Maximum directory depth to display (default: 10)")
    
    args = parser.parse_args()
    
    project_path = os.path.abspath(args.project_path)
    
    if not os.path.exists(project_path):
        print(f"Error: The specified path does not exist: {project_path}")
        sys.exit(1)
        
    if not os.path.isdir(project_path):
        print(f"Error: The specified path is not a directory: {project_path}")
        sys.exit(1)
    
    # Display project info
    project_name = os.path.basename(project_path)
    print(f"\033[36mProject: {project_name}")
    print(f"Path: {project_path}")
    print(f"Date: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    print("--------------------------------------\033[0m")
    print()
    
    # Start the analysis
    get_project_structure(project_path, max_depth=args.depth, include_files=args.files)
    
    print()
    print(f"\033[90mExcluded directories: {', '.join(sorted(IGNORE_DIRS))}")
    print(f"Excluded file extensions: {', '.join(sorted(IGNORE_EXTENSIONS))}")
    print(f"Excluded files: {', '.join(sorted(IGNORE_FILES))}\033[0m")


if __name__ == "__main__":
    main()
