:remove_from_path -- remove %~2 from %~1
SETLOCAL ENABLEDELAYEDEXPANSION
set P=!%~1!
set P=!P:%~2=!
set P=!P:;;=;!
set P=!P:;;=;!
(ENDLOCAL & REM.-- RETURN VALUES
  SET "%~1=%P%"
)
exit /b