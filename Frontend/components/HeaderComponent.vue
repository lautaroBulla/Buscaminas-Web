<script setup>
  //manejda la logica de los elementos del header (help, mines, reset, timer, settings)
  defineProps({
    seconds: {
      type: Number,
      required: true
    },
    remainingMines: {
      type: Number,
      required: true
    },
    gameOver: {
      type: Boolean,
      required: true
    },
    gameWin: {
      type: Boolean,
      required: true
    }
  });
</script>

<template>
  <div class="flex flex-row items-center border-internal">
    <div class="flex justify-start w-1/5 m-1">
      <HelpComponent 
        @help="$emit('help')"
      />
    </div>
    <div class="flex justify-center w-1/5 m-1">
      <RemainingMinesComponent
        :remainingMines="remainingMines"
      />
    </div>
    <div class="flex justify-center w-1/5 m-1">
      <ResetComponent
        :gameOver="gameOver"
        :gameWin="gameWin"
        @restart-game="$emit('restart-game')"
      />
    </div>
    <div class="flex justify-center w-1/5 m-1">
      <TimerComponent
        :seconds="seconds"
      />
    </div>
    <div class="flex justify-end w-1/5 m-1">
      <SettingsComponent 
        @view-settings="$emit('view-settings')"
      />
    </div>
  </div>
</template>