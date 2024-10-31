package com.immichframe.immichframe

import android.content.Context
import android.content.Intent
import android.os.PowerManager
import android.service.dreams.DreamService

class ScreenSaverService : DreamService() {
    private var wakeLock: PowerManager.WakeLock? = null

    override fun onDreamingStarted() {
        super.onDreamingStarted()
        acquireWakeLock()
        val activityIntent = Intent(this, MainActivity::class.java)
        activityIntent.addFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT or Intent.FLAG_ACTIVITY_NEW_TASK)
        startActivity(activityIntent)
    }

    override fun onDreamingStopped() {
        super.onDreamingStopped()
        releaseWakeLock()
    }
    override fun onDestroy() {
        super.onDestroy()
        releaseWakeLock()
    }
    fun acquireWakeLock() {
        if (wakeLock == null) {
            val powerManager = getSystemService(Context.POWER_SERVICE) as PowerManager
            wakeLock = powerManager.newWakeLock(
                PowerManager.PARTIAL_WAKE_LOCK,"ImmichFrame::ScreenSaverWakeLock")
            wakeLock?.acquire(120 * 60 * 1000L)
        }
    }
    fun releaseWakeLock() {
        wakeLock?.let {
            if (it.isHeld) {
                it.release()
            }
        }
        wakeLock = null
    }
}