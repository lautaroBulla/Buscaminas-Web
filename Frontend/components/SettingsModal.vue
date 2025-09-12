<script setup>
  import closeClassic from '~/assets/img/themes/classicTheme/close.png';
  import closeDark from '~/assets/img/themes/darkTheme/closeDark.png';

  import esFlag from '~/assets/img/es.png'
  import ukFlag from '~/assets/img/uk.png'

  import faceClassic from '~/assets/img/themes/classicTheme/face.png';
  import mineClasssic from '~/assets/img/themes/classicTheme/mine.png';
  import flagClasssic from '~/assets/img/themes/classicTheme/flag.png';
  import oneClasssic from '~/assets/img/themes/classicTheme/one.png';

  import faceDark from '~/assets/img/themes/darkTheme/faceDark.png';
  import mineDark from '~/assets/img/themes/darkTheme/mineDark.png';
  import flagDark from '~/assets/img/themes/darkTheme/flagDark.png';
  import oneDark from '~/assets/img/themes/darkTheme/oneDark.png';

  import { ref } from 'vue';
  
  const props = defineProps({
    firstClickZero: {
      type: Boolean,
      required: true
    },
    interrogationsActivated: {
      type: Boolean,
      required: true
    }
  });

  const { locale } = useI18n();
  const language = ref(locale.value);

  const { currentTheme } = useCurrentTheme();
  const theme = ref(currentTheme.value);
	
	const imgByTheme = {
    classicTheme: {
      close: closeClassic
    },
		darkTheme: {
			close: closeDark
		}
  };

  const currentThemeComputed = computed(() => {
    return currentTheme.value;
  });

  const localFirstClickZero = ref(props.firstClickZero);
  const localInterrogationsActivated = ref(props.interrogationsActivated);

  const emit = defineEmits(['update', 'close']);

  const updateSettings = () => {
    emit('update', {
      language: language.value,
      theme: theme.value,
      firstClickZero: localFirstClickZero.value,
      interrogationsActivated: localInterrogationsActivated.value
    });
  }
</script>

<template>

  <div class="fixed inset-0 flex items-center justify-center bg-black/50 z-50">
    <div class="border-external w-full max-w-xl m-1">
      <div class="border">
        <div class="border-internal p-2">

          <div class="flex justify-end">
            <div class="button-border">
              <button @click="emit('close')" 
                      class="button w-[20px] h-[20px]
                              md:w-[25px] md:h-[25px]
                              lg:w-[30px] lg:h-[30px]"
              >
                <img :src="imgByTheme[currentThemeComputed].close" alt="Close"/>
              </button>
            </div>
          </div>

          <div class="flex flex-col">

            <div class="flex flex-col gap-y-1">

              <div class="md:hidden
                          flex flex-col"
              >
                <label class="text-color"> {{ $t('gameSettings.language') }}: </label>
                <div class="flex flex-row space-x-3">

                  <div :class="language !== 'en' ? 'opacity-50 button-border-2' : 'button-border'">
                    <button class="gap-x-2 px-2 h-[30px]
                                  md:h-[35px]"
                            :class="language !== 'en' ? 'button-2' : 'button'"
                            @click="language = 'en'"
                    > 
                      {{ $t('gameSettings.english') }}  
                      <img :src="ukFlag" />
                    </button>
                  </div>
                  
                  <div :class="language !== 'es' ? 'opacity-50 button-border-2' : 'button-border'">
                    <button class="gap-x-2 px-2 h-[30px]
                                  md:h-[35px]"
                            :class="language !== 'es' ? 'button-2' : 'button'"
                            @click="language = 'es'"
                    > 
                      {{ $t('gameSettings.spanish') }} 
                      <img :src="esFlag" />
                    </button>
                  </div>
                </div>
              </div>

              <div class="flex flex-col">
                <label class="text-color"> {{ $t('gameSettings.theme') }}: </label>
                <div class="grid grid-cols-2 
                            md:grid-cols-3 
                            lg:grid-cols-4"
                >
                  <div :class="theme !== 'classicTheme' ? 'button-border-2' : 'button-border'">
                    <button class="gap-x-2 px-2 py-1 h-[30px]
                                  md:h-[35px]
                                  lg:h-[40px]"
                            :class="theme !== 'classicTheme' ? 'opacity-50 modal-button-classic-2' : 'modal-button-classic'"
                            @click="theme = 'classicTheme'"
                    > 
                      <img :src="faceClassic" class="w-5 h-5" />
                      <img :src="mineClasssic" class="w-5 h-5" />
                      <img :src="flagClasssic" class="w-3 h-4" />
                      <img :src="oneClasssic" class="w-3 h-4" />
                    </button>
                  </div>
                  
                  <div :class="theme !== 'darkTheme' ? 'button-border-2' : 'button-border'">
                    <button class="gap-x-2 px-2 py-1 h-[30px]
                                  md:h-[35px]
                                  lg:h-[40px]"
                            :class="theme !== 'darkTheme' ? 'opacity-50 modal-button-dark-2' : 'modal-button-dark'"
                            @click="theme = 'darkTheme'"
                    > 
                      <img :src="faceDark" class="w-5 h-5" />
                      <img :src="mineDark" class="w-5 h-5" />
                      <img :src="flagDark" class="w-3 h-4" />
                      <img :src="oneDark" class="w-3 h-4" />
                    </button>
                  </div>

                </div>
              </div>

            </div>
            
            <div class="modal-separator-line my-4"></div>

            <div class="flex flex-col gap-y-1">

              <div class="flex flex-row items-center gap-x-2">
                <div :class="localFirstClickZero ? 'button-border' : 'button-border-2'">
                  <input 
                    type="checkbox" 
                    class="checkbox" 
                    :class="localFirstClickZero ? 'button' : 'button-2'"
                    v-model="localFirstClickZero" 
                  />
                </div>
                <label class="text-color"> {{ $t('gameSettings.firstClick') }} </label>
              </div>

              <div class="hidden md:flex flex-row items-center gap-x-2">
                <div :class="localInterrogationsActivated ? 'button-border' : 'button-border-2'">
                  <input 
                    type="checkbox" 
                    class="checkbox" 
                    :class="localInterrogationsActivated ? 'button' : 'button-2'" 
                    v-model="localInterrogationsActivated" 
                  />
                  </div>
                <label class="text-color"> {{ $t('gameSettings.interrogations') }} </label>
              </div>

            </div>

          </div>

          <div class="flex justify-center mt-2">
            <div class="button-border">
              <button 
                @click="updateSettings"
                class="button px-2 py-1 h-[30px]
                      md:h-[35px]
                      lg:h-[40px]"
              >
                {{ $t('gameSettings.update') }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>