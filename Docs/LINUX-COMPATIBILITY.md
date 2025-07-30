# Linux Compatibility Fixes

This document outlines the changes made to ensure the UGH project works correctly on Linux environments.

## Issues Addressed

### 1. Docker Container Naming Inconsistency
**Problem**: Auto-generated container names caused issues with scripts that expected consistent naming across platforms.

**Solution**: Added fixed `container_name` fields to all services in `compose.yaml`:
- `ugh-webserver` (nginx)
- `ugh-frontend` (vuetify frontend)
- `ugh-backend` (.NET API)
- `ugh-db` (MySQL database)
- `ugh-migration` (database migration service)

### 2. Environment Variable Persistence in Docker
**Problem**: Environment variables set in `docker exec` sessions don't persist across different exec calls on Linux.

**Solution**: Implemented file-based token system for admin reset functionality:
- **PowerShell Script**: `scripts/powershell/secure-admin-setup.ps1` now writes tokens to `/tmp/admin_reset_token`
- **Backend Controller**: `Backend/Controllers/AdminSetupController.cs` reads tokens from file instead of environment variables

### 3. Cross-Platform PowerShell Support
**Problem**: Scripts needed to work on both Windows PowerShell and PowerShell Core (Linux/macOS).

**Solution**: 
- Added platform detection using `$IsLinux` and `$IsMacOS` variables
- Included platform-specific usage examples in help text
- Used cross-platform compatible file paths and commands

### 4. PowerShell Variable Conflicts
**Problem**: PowerShell Core has built-in variables `$IsWindows`, `$IsLinux`, `$IsMacOS` that should not be overwritten.

**Solution**: 
- Use custom variable names like `$ScriptIsWindows`, `$ScriptIsLinux` to avoid conflicts
- Preserve the original PowerShell variables for other scripts and modules
- This prevents unexpected behavior and maintains compatibility

## Files Modified

### Core Files
1. `compose.yaml` - Added container_name fields
2. `scripts/powershell/secure-admin-setup.ps1` - File-based token system
3. `Backend/Controllers/AdminSetupController.cs` - File reading instead of environment variables
4. `ADMIN-SECURITY.md` - Updated container name references

### Scripts Already Compatible
- `scripts/powershell/migrate-db.ps1` - Uses service names, not container names
- `scripts/powershell/update-ports.ps1` - Already cross-platform compatible

## Testing Recommendations

To verify Linux compatibility:

1. **Test Container Names**:
   ```bash
   docker-compose up -d
   docker ps --format "table {{.Names}}\t{{.Image}}"
   ```

2. **Test Admin Reset**:
   ```bash
   ./scripts/powershell/secure-admin-setup.ps1 -NewPassword "TestPassword123!"
   ```

3. **Test File Token System**:
   ```bash
   # Check if token file is created
   docker exec ugh-backend ls -la /tmp/admin_reset_token
   
   # Check if backend can read the file
   docker exec ugh-backend cat /tmp/admin_reset_token
   ```

## Platform-Specific Notes

### Linux/macOS
- Use PowerShell Core (`pwsh` command)
- File paths use forward slashes in Docker containers
- Token file location: `/tmp/admin_reset_token`

### Windows
- Can use Windows PowerShell or PowerShell Core
- Docker Desktop handles path translation
- Same token file location works due to Linux containers

## Security Considerations

- Token files are created in `/tmp` with restricted access
- Tokens are automatically cleaned up after use
- File-based approach is more secure than environment variables in shared containers
