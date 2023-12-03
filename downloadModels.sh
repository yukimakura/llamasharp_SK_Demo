mkdir models
aria2c -x16 -s16 -o models/xwin-lm-13b-v0.2.Q3_K_S.gguf  \
        https://huggingface.co/TheBloke/Xwin-LM-13B-v0.2-GGUF/resolve/main/xwin-lm-13b-v0.2.Q3_K_S.gguf
aria2c -x16 -s16 -o models/xwincoder-13b.Q3_K_S.gguf  \
        https://huggingface.co/TheBloke/XwinCoder-13B-GGUF/resolve/main/xwincoder-13b.Q3_K_S.gguf

